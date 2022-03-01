using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestAPI.Models;

namespace TestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MusterilerController : ControllerBase
    {

        private static Random random = new Random();
        public List<string> cities = new List<string>() { "Ankara", "İstanbul", "İzmir", "Bursa", "Edirne", "Konya", "Antalya", "Diyarbakır", "Van", "Rize" };
        [HttpGet]
        public IEnumerable<Musteri> Get()
        {
            using (var context = new testdbContext())
            {
                return context.Musteri.ToList();
            }
        }

        [HttpGet("getbyid/{Id}")]
        public IEnumerable<Musteri> GetById(int Id)
        {
            using (var context = new testdbContext())
            {
                return context.Musteri.ToList().Where(musteri => musteri.Id == Id);
            }
        }

        [HttpPost]
        public IEnumerable<Musteri> Post(Musteri musteri)
        {
            using (var context = new testdbContext())
            {
                context.Add(musteri);

                context.SaveChanges();

                return context.Musteri.ToList();
            }
        }

        [HttpPost("TestVerisiOlustur")]
        public IEnumerable<Musteri> TestVerisiOlustur([FromQuery] int musteriAdet, int sepetAdet)
        {
            using (var context = new testdbContext())
            {
                List<Musteri> musteriList = new List<Musteri>();
                List<Sepet> sepetList = new List<Sepet>();
                for (int i = 0; i < musteriAdet; i++)
                {
                    int index = random.Next(cities.Count);
                    Musteri musteri = new Musteri();
                    musteri.Ad = RandomString(5);
                    musteri.Soyad = RandomString(5);
                    musteri.Sehir = cities[index];
                    context.Add(musteri);
                    context.SaveChanges();
                    musteriList.Add(musteri);
                }

                for (int i = 0; i < sepetAdet; i++)
                {
                    int index = random.Next(musteriList.Count);
                    Sepet sepet = new Sepet();
                    sepet.MusteriId = musteriList[index].Id;
                    context.Add(sepet);
                    context.SaveChanges();
                    sepetList.Add(sepet);
                }

                foreach (var sepet in sepetList)
                {
                    int urunAdet = random.Next(5) + 1;
                    for (int i = 0; i < urunAdet; i++)
                    {
                        SepetUrun sepetUrun = new SepetUrun();
                        sepetUrun.Tutar = random.Next(100, 1000);
                        sepetUrun.Aciklama = RandomString(5);
                        sepetUrun.SepetId = sepet.Id;
                        context.Add(sepetUrun);
                        context.SaveChanges();
                    }
                }

                return context.Musteri.ToList();
            }
        }

        [HttpGet("SehirBazliAnalizYap")]
        public IEnumerable<DtoSehirAnaliz> SehirBazliAnalizYap()
        {
            List<DtoSehirAnaliz> sehirAnalizList = new List<DtoSehirAnaliz>();

            using (var context = new testdbContext())
            {
                Dictionary<string, List<int>> sehirMusteriList = new Dictionary<string, List<int>>();
                Dictionary<int, List<int>> musteriSepetList = new Dictionary<int, List<int>>();
                Dictionary<int, decimal> sepetTutarList = new Dictionary<int, decimal>();

                List<Musteri> musteriList = context.Musteri.ToList();
                List<Sepet> sepetList = context.Sepet.ToList();
                List<SepetUrun> sepetUrunList = context.SepetUrun.ToList();

                foreach (var musteri in musteriList)
                {
                    sehirMusteriList[musteri.Sehir] = musteriList.Where(x => x.Sehir == musteri.Sehir).Select(x => x.Id).ToList();
                }
                foreach (var musteri in musteriList)
                {
                    musteriSepetList[musteri.Id] = sepetList.Where(x => x.MusteriId == musteri.Id).Select(x => x.Id).ToList();
                }
                foreach (var sepet in sepetList)
                {
                    sepetTutarList[sepet.Id] = sepetUrunList.Where(x => x.SepetId == sepet.Id).Sum(x => x.Tutar);
                }

                foreach (var sehirMusteri in sehirMusteriList)
                {
                    DtoSehirAnaliz sehirAnaliz = new DtoSehirAnaliz();
                    sehirAnaliz.SehirAdi = sehirMusteri.Key;

                    foreach (var musteriId in sehirMusteri.Value)
                    {
                        foreach (var musteriSepet in musteriSepetList.Where(x => x.Key == musteriId).ToList())
                        {
                            sehirAnaliz.SepetAdet += musteriSepet.Value.Count;
                            foreach (var sepetId in musteriSepet.Value)
                            {
                                sehirAnaliz.ToplamTutar += sepetTutarList.Where(x => x.Key == sepetId).FirstOrDefault().Value;
                            }
                        }
                    }
                    sehirAnalizList.Add(sehirAnaliz);
                }
            }
            return sehirAnalizList.OrderByDescending(x=>x.SepetAdet);
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
