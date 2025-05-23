// File: FlightRegistration.WinFormsClient/BoardingPassPrinter.cs
using System;
using System.Drawing;
using System.Drawing.Imaging; // For ImageFormat
using System.Drawing.Printing;
using System.Windows.Forms;
using FlightRegistration.Core.DTOs;
using ZXing; // Main ZXing namespace
using ZXing.Common; // For EncodingOptions
using ZXing.QrCode;
using ZXing.Windows.Compatibility; // For BarcodeWriter

namespace FlightRegistration.WinFormsClient
{
    internal class BoardingPassPrinter
    {
        private BoardingPassDto _boardingPassToPrint;
        private Font _titleFont, _headerFont, _detailFont, _smallFont;
        private Font _barcodeDataFont; // For text under barcode

        public BoardingPassPrinter()
        {
            try
            {
                _titleFont = new Font("8-bit Operator+ SC", 19, FontStyle.Bold);
                _headerFont = new Font("8-bit Operator+ SC", 15, FontStyle.Bold);
                _detailFont = new Font("8-bit Operator+ SC", 13, FontStyle.Regular);
                _smallFont = new Font("8-bit Operator+ SC", 11, FontStyle.Italic);
                // Attempt to use "8-bit Operator+ SC" for barcode data text
                _barcodeDataFont = new Font("8-bit Operator+ SC", 15, FontStyle.Regular);
            }
            catch (ArgumentException) // Fallback if 8-bit Operator+ SC or 8-bit Operator+ SC is not found
            {
                _titleFont = new Font("Arial", 16, FontStyle.Bold);
                _headerFont = new Font("Arial", 12, FontStyle.Bold);
                _detailFont = new Font("Arial", 10, FontStyle.Regular);
                _smallFont = new Font("Arial", 8, FontStyle.Italic);
                _barcodeDataFont = new Font("Consolas", 8, FontStyle.Regular); // Fallback for barcode data
                Console.WriteLine("8-bit Operator+ SC or 8-bit Operator+ SC font not found for printing, using fallbacks.");
            }
        }

        public void PrintBoardingPass(BoardingPassDto boardingPass, bool showPreview = true)
        {
            // ... (null check for boardingPass remains the same) ...
            if (boardingPass == null) { /* ... */ return; }
            _boardingPassToPrint = boardingPass;

            PrintDocument printDocument = new PrintDocument();
            printDocument.DocumentName = $"BoardingPass_{_boardingPassToPrint.PassengerName?.Replace(" ", "_")}_{_boardingPassToPrint.FlightNumber}";
            printDocument.PrintPage += PrintDocument_PrintPage;

            if (showPreview)
            {
                using (PrintPreviewDialog previewDialog = new PrintPreviewDialog())
                {
                    previewDialog.Document = printDocument;
                    previewDialog.StartPosition = FormStartPosition.CenterScreen;
                    previewDialog.WindowState = FormWindowState.Maximized;
                    try { previewDialog.ShowDialog(); }
                    catch (Exception ex) { MessageBox.Show($"Print preview error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
            else { /* ... direct PrintDialog logic ... */ }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_boardingPassToPrint == null) return;

            Graphics graphics = e.Graphics;
            float yPos = e.MarginBounds.Top;
            float leftMargin = e.MarginBounds.Left;
            float printableWidth = e.MarginBounds.Width;
            // ... (lineHeightDetail, lineHeightHeader, centerFormat, rightFormat as before) ...
            StringFormat centerFormat = new StringFormat { Alignment = StringAlignment.Center };


            // --- Header ---
            string airlineName = "AND";
            graphics.DrawString(airlineName, _titleFont, Brushes.DarkBlue, leftMargin + printableWidth / 2, yPos, centerFormat);
            yPos += _titleFont.GetHeight(graphics) * 1.5f;
            graphics.DrawString("BOARDING PASS", _headerFont, Brushes.Black, leftMargin + printableWidth / 2, yPos, centerFormat);
            yPos += _headerFont.GetHeight(graphics) * 2;

            // --- Flight Details ---
            DrawDetailRow(graphics, "PASSENGER:", _boardingPassToPrint.PassengerName?.ToUpper(), ref yPos, leftMargin, printableWidth);
            DrawDetailRow(graphics, "FLIGHT:", _boardingPassToPrint.FlightNumber, ref yPos, leftMargin, printableWidth);
            DrawDetailRow(graphics, "FROM:", _boardingPassToPrint.DepartureCity, ref yPos, leftMargin, printableWidth);
            DrawDetailRow(graphics, "TO:", _boardingPassToPrint.ArrivalCity, ref yPos, leftMargin, printableWidth);
            yPos += _detailFont.GetHeight(graphics) * 0.5f;
            DrawDetailRow(graphics, "DEPARTURE:", _boardingPassToPrint.DepartureTime.ToString("dd MMM yyyy HH:mm"), ref yPos, leftMargin, printableWidth);
            DrawDetailRow(graphics, "BOARDING TIME:", _boardingPassToPrint.BoardingTime.ToString("HH:mm"), ref yPos, leftMargin, printableWidth);
            DrawDetailRow(graphics, "SEAT:", _boardingPassToPrint.SeatNumber, ref yPos, leftMargin, printableWidth, true);
            yPos += _detailFont.GetHeight(graphics) * 2;

            // --- Barcode Generation and Drawing ---
            string barcodeData = $"{_boardingPassToPrint.FlightNumber}-{_boardingPassToPrint.SeatNumber}-{_boardingPassToPrint.PassengerName?.Split(' ').LastOrDefault() ?? "LNAME"}";
            // Make barcodeData simpler if needed, e.g., just Booking ID if you had it.
            // Code 128 is good for alphanumeric.

            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Height = 60, // pixels
                    Width = 250, // pixels
                    Margin = 5, //
                    PureBarcode = false, // Set to true if you don't want text below barcode image itself
                }
                // Renderer = new BitmapRenderer() // ZXing.Net.Bindings.Windows.Compatibility often handles this
            };

            try
            {
                using (Bitmap barcodeBitmap = writer.Write(barcodeData))
                {
                    float barcodeX = leftMargin + (printableWidth - barcodeBitmap.Width) / 2; // Center barcode
                    graphics.DrawImage(barcodeBitmap, barcodeX, yPos);
                    yPos += barcodeBitmap.Height;

                    // Optionally draw the barcode data text underneath using your desired font
                    if (!writer.Options.PureBarcode) // If ZXing didn't already render text
                    {
                        yPos += 2; // Small gap
                        graphics.DrawString(barcodeData, _barcodeDataFont, Brushes.Black, barcodeX + barcodeBitmap.Width / 2, yPos, centerFormat);
                        yPos += _barcodeDataFont.GetHeight(graphics);
                    }
                }
            }
            catch (Exception ex)
            {
                graphics.DrawString($"Barcode Error: {ex.Message}", _smallFont, Brushes.Red, leftMargin, yPos);
                yPos += _smallFont.GetHeight(graphics) * 2;
            }
            // --- End Barcode ---


            yPos += _detailFont.GetHeight(graphics);
            string footerText = "HAVE A PLEASANT FLIGHT!";
            graphics.DrawString(footerText, _headerFont, Brushes.DarkBlue, leftMargin + printableWidth / 2, yPos, centerFormat);
            yPos += _headerFont.GetHeight(graphics) * 1.5f;

            graphics.DrawString($"Printed: {DateTime.Now:g}", _smallFont, Brushes.Gray, leftMargin, yPos);
            e.HasMorePages = false;
        }

        private void DrawDetailRow(Graphics g, string label, string value, ref float y, float x, float width, bool highlightValue = false)
        {
            Font valueFont = highlightValue ? _headerFont : _detailFont;
            Brush valueBrush = highlightValue ? Brushes.DarkRed : Brushes.Black;
            float valueX = x + 150;
            g.DrawString(label, _detailFont, Brushes.Gray, x, y);
            g.DrawString(value ?? "N/A", valueFont, valueBrush, valueX, y);
            y += Math.Max(_detailFont.GetHeight(g), valueFont.GetHeight(g)) * 1.2f;
        }
    }
}