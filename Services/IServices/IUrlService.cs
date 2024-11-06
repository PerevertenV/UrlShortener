using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IUrlService
    {
        public KeyValuePair<string, int> CreateShortUrl(string domen);

        public KeyValuePair<string, int>? GenerateRandomTask();
    }

}
