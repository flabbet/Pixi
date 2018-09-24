﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using PixiEditor.Models.Enums;

namespace PixiEditor.Models
{
    public class Exporter
    {
        public static string _savePath = null;
        public static Size _fileDimensions;

        /// <summary>
        /// Creates ExportFileDialog to get width, height and path of file.
        /// </summary>
        /// <param name="type">Type of file to be saved in.</param>
        /// <param name="imageToSave">Image to be saved as file.</param>
        public static void Export(FileType type,Image imageToSave)
        {
                ExportFileDialog info = new ExportFileDialog();
                //If OK on dialog has been clicked
                if (info.ShowDialog() == true)
                {
                    //If sizes are incorrect
                    if(info.FileWidth < imageToSave.Width || info.FileHeight < imageToSave.Height)
                    {
                        MessageBox.Show("Incorrect height or width value", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    _savePath = info.FilePath;
                    _fileDimensions = new Size(info.FileWidth, info.FileHeight);
                    SaveAsPng(info.FilePath, (int)imageToSave.Width, (int)imageToSave.Height, info.FileHeight, info.FileWidth, imageToSave);
                }
        }

        /// <summary>
        /// Saves file with info that has been recieved from ExportFileDialog before, doesn't work without before Export() usage.
        /// </summary>
        /// <param name="type">Type of file</param>
        /// <param name="imageToSave">Image to be saved as file.</param>
        public static void ExportWithoutDialog(FileType type, Image imageToSave)
        {
            try
            {
                SaveAsPng(_savePath, (int)imageToSave.Width, (int)imageToSave.Height, (int)_fileDimensions.Height, (int)_fileDimensions.Width, imageToSave);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Saves image to PNG file
        /// </summary>
        /// <param name="savePath">Save file path</param>
        /// <param name="originalWidth">Original width of image</param>
        /// <param name="originalHeight">Original height of image</param>
        /// <param name="exportWidth">File width</param>
        /// <param name="exportHeight">File height</param>
        /// <param name="imageToExport">Image to be saved</param>
        private static void SaveAsPng(string savePath, int originalWidth, int originalHeight, int exportWidth, int exportHeight, Image imageToExport)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(imageToExport);
            double dpi = 96d;


            RenderTargetBitmap rtb = new RenderTargetBitmap(originalWidth * (exportWidth / originalWidth), originalHeight * (exportHeight/originalHeight), dpi, dpi, PixelFormats.Default);


            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(imageToExport);
                dc.DrawRectangle(vb, null, new Rect(new Point(), new Size(originalWidth * (exportWidth / originalWidth), originalHeight * (exportHeight / originalHeight))));
            }

            rtb.Render(dv);
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            try
            {
                MemoryStream ms = new MemoryStream();

                pngEncoder.Save(ms);
                ms.Close();

                File.WriteAllBytes(savePath, ms.ToArray());
            }
            catch (Exception err)
            {
                System.Windows.MessageBox.Show(err.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
