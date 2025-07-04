using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace HotelManagementSystem
{
    public static class clsUtility
    {
        public static string GenerateNewGuid()
        {
            return Guid.NewGuid().ToString();
        }

        public static bool CreateFolderIfDoesNotExist(string FolderPath)
        {
            if(!Directory.Exists(FolderPath))
            {
                try
                {
                    Directory.CreateDirectory(FolderPath);
                    return true;
                }

                catch(Exception ex)
                {
                    MessageBox.Show("Error occured while creating folder : "+ ex.Message);
                    return false;
                }
            }

            return true;
        }

        public static string ReplaceFileNameWithGuid(string SourceFile)
        {          
            FileInfo fileInfo = new FileInfo(SourceFile);

            // replace the file name but keep the extension type
            return GenerateNewGuid() + fileInfo.Extension;

        }

        public static bool CopyImageToProjectImagesFolder(string DestinationFolder , ref string SourceFile)
        {
            if (!CreateFolderIfDoesNotExist(DestinationFolder))
                return false;

            string DestinationFile = DestinationFolder + ReplaceFileNameWithGuid(SourceFile); 

            try
            {
                File.Copy(SourceFile, DestinationFile);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }

            SourceFile = DestinationFile;
            return true;
        }

        public static void RemoveIconAnimation(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is Guna2Button btn)
                {
                    if (btn.HoverState.Image != null)
                        btn.Image = btn.HoverState.Image;

                    btn.HoverState.Image = btn.Image;
                }
                else if (ctrl is Guna2ImageButton imgBtn)
                {
                    if (imgBtn.HoverState.Image != null)
                        imgBtn.Image = imgBtn.HoverState.Image;
                    imgBtn.HoverState.Image = imgBtn.Image;
                }

                if (ctrl.HasChildren)
                    RemoveIconAnimation(ctrl);
            }
        }

    }
}
