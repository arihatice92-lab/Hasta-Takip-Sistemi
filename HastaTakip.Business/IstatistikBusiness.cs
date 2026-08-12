using System.Collections.Generic;
using HastaTakip.DataAccess;
using HastaTakip.Entities;

namespace HastaTakip.Business
{
    public class IstatistikBusiness
    {
        private readonly IstatistikDal _dal;
        public IstatistikBusiness(IstatistikDal dal) { _dal = dal; }

        public List<TaniCinsiyetIstatistigi> TaniCinsiyetIstatistigiGetir() => _dal.TaniCinsiyetIstatistigiGetir();
    }
}