using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gifter.Services.DTOS.Image
{
    public class DownloadImageDTO
    {
        public string FileExtension { get; set; }

        public byte[] Image { get; set; }
    }
}
