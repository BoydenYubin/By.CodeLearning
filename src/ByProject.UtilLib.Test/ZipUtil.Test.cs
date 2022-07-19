using ByProject.UtilLib.ZipUtil;
using Shouldly;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Faker;
using System.Text;

namespace ByProject.UtilLib.Test
{
    public class ZipUtilTest
    {
        private readonly string testPath = @"D:\ZipUtilTest";
        private string nameFile;
        private string addFile;
        private string zipFile;
        private readonly string comment = "The file was zipped by test!!!";
        private readonly string pass = "easy code";
        public ZipUtilTest()
        {
            nameFile = Path.Combine(testPath, "name.txt");
            addFile = Path.Combine(testPath, "address.txt");
            zipFile = Path.Combine(testPath, "zipUtil.zip");
        }
        [Fact(DisplayName = "1. 添加种子数据")]
        public void SeedFile()
        {
            if (!Directory.Exists(testPath))
            {
                Directory.CreateDirectory(testPath);
            }
            StringBuilder addressBuilder = new StringBuilder();
            StringBuilder nameBuilder = new StringBuilder();
            for (int i = 0; i < 100000; i++)
            {
                addressBuilder.Append(Address.Country() + " ");
                addressBuilder.Append(Address.CaProvince() + " ");
                addressBuilder.Append(Address.StreetAddress() + " ");
                addressBuilder.Append(Address.UkPostCode());
                addressBuilder.AppendLine();
                nameBuilder.Append(Name.First() + " ");
                nameBuilder.Append(Name.Middle() + " ");
                nameBuilder.Append(Name.Last());
                nameBuilder.AppendLine();
            }
            File.WriteAllText(nameFile, nameBuilder.ToString());
            File.WriteAllText(addFile, addressBuilder.ToString());
            File.Exists(nameFile).ShouldBeTrue();
            File.Exists(addFile).ShouldBeTrue();
        }
        [Fact(DisplayName = "2. 压缩一个文件")]
        public void CompressFileShouldSuccess()
        {
            var result = ZipHelper.CompressFile(path: addFile,
               zipFilePath: zipFile, comment: comment, password: pass, compressionLevel: 6);
            result.ShouldBeTrue();
            File.Exists(zipFile).ShouldBeTrue();
        }
        [Fact(DisplayName = "3. 压缩多个文件")]
        public void CompressDirectoryShouldSuccess()
        {
            var result = ZipHelper.CompressFile(sourceList: new List<string>() { addFile, nameFile },
                zipFilePath: zipFile,
                comment: comment,
                password: pass,
                compressionLevel: 6);
            result.ShouldBeTrue();
            File.Exists(zipFile).ShouldBeTrue();
        }
        [Fact(DisplayName = "4. 解压文件")]
        public void DecomparessFileShouldSuccess()
        {
            string decomparessFilePath = Path.Combine(testPath, "DecomparessFile");
            var result = ZipHelper.DecomparessFile(zipFile, destinationDirectory: decomparessFilePath);
            result.ShouldBeTrue();
            File.Exists(Path.Combine(decomparessFilePath, "name.txt")).ShouldBeTrue();
            File.Exists(Path.Combine(decomparessFilePath, "address.txt")).ShouldBeTrue();
        }
        [Fact(DisplayName = "5. 压缩多个文件")]
        public void DeleteFile()
        {
            Directory.Delete(testPath);
            Directory.Exists(testPath).ShouldBeFalse();
        }
    }
}