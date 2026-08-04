using HastaTakip.DataAccess;
using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Business
{
    public class PsikologBusiness
    {
        private readonly PsikologDal _psikologDal;
        public PsikologBusiness(PsikologDal psikologDal) { _psikologDal = psikologDal; }
        public List<Psikolog> PsikologlariListele() => _psikologDal.PsikologlariListele();
    }
}
