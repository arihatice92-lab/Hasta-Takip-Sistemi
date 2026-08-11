
using System.Collections.Generic;
using HastaTakip.DataAccess;
using HastaTakip.Entities;

namespace HastaTakip.Business
{
    public class PsikologRandevuSaatBusiness
    {
        private readonly PsikologRandevuSaatDal _dal;
        public PsikologRandevuSaatBusiness(PsikologRandevuSaatDal dal) { _dal = dal; }
        public List<PsikologRandevuSaat> SaatleriListele() => _dal.SaatleriListele();
    }
}