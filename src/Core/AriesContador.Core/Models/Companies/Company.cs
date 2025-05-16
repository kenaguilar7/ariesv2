﻿using System;
using AriesContador.Core.Models.Utils;
using System.Collections;
using System.Collections.Generic;
using AriesContador.Core.Models.PostingPeriods;
using AriesContador.Core.Models.Accounts;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AriesContador.Core.Models.Companies
{
    public class Company : BaseModel
    {
        public bool IsActive { get; set; } = true; 
        public int UserId { get; set; }
        public string CopyFrom { get; set; }
        public string Code { get; set; }

        public string CompanyName { get; set; }

        public CompanyType CompanyType { get; set;  }

        public IdType IdType { get; set; }

        public string NumberId { get; set; }

        public string Op1 { get; set; }

        public string Op2 { get; set; }

        public string Address { get; set; }

        public string Mail { get; set; }

        public string PhoneNumber1 { get; set; }

        public string PhoneNumber2 { get; set; }

        public string Notes { get; set; }

        public string WebSite { get; set; }

        public CurrencyTypeCompany MoneyType { get; set; }

        public IEnumerable<PostingPeriod> PostingPeriods { get; set; } = new List<PostingPeriod>();

        public IEnumerable<Account> Account { get; set; } = new List<Account>(); 

        public override string ToString()
        {
            return $"{ CompanyName.ToUpper()}-{Code}";
        }

        public Company() { }
        protected Company(IdType tipoID, string numeroId, string nombre, CurrencyTypeCompany TipoMoneda, string direccion,
                         string[] telefono, string web, string correo, string observaciones, string codigo = "", Boolean activo = true)
        {
            this.Code = codigo;
            this.IdType = tipoID;
            this.NumberId = numeroId;
            this.CompanyName = nombre;
            this.MoneyType = TipoMoneda;
            this.Address = direccion;
            this.PhoneNumber1 = telefono[0];
            this.PhoneNumber2 = telefono[1];
            this.WebSite = web;
            this.Mail = correo;
            this.Notes = observaciones;
            this.Active = activo;
        }
    }

    public class PersonaFisica : Company
    {
        private String apellidoPaterno;
        private String apellidoMaterno;


        public PersonaFisica()
        {
        }

        public PersonaFisica(IdType tipoID, string numeroId, string nombre, CurrencyTypeCompany TipoMoneda, string direccion,
                                string[] telefono, string web, string correo, string observaciones, String apellidoPaterno, String apellidoMaterno, string codigo = "", Boolean activo = true) :
                                base(tipoID, numeroId, nombre, TipoMoneda, direccion, telefono, web, correo, observaciones, codigo, activo)
        {
            this.apellidoPaterno = apellidoPaterno;
            this.apellidoMaterno = apellidoMaterno;
        }
        //public PersonaFisica(String apelledoPaterno, String apellidoMaterno)
        //{
        //    this.apellidoPaterno = apelledoPaterno;
        //    this.MyApellidoMaterno = apellidoMaterno;
        //}

        public String MyApellidoPaterno
        {
            get { return apellidoPaterno; }
            set { apellidoPaterno = value; }
        }

        public String MyApellidoMaterno
        {
            get { return apellidoMaterno; }
            set { apellidoMaterno = value; }
        }


    }

    public class PersonaJuridica : Company
    {
        private String representanteLegal;
        private String IDRepresentante;
        public PersonaJuridica()
        {
        }

        public PersonaJuridica(IdType tipoID, string numeroId, string nombre, CurrencyTypeCompany TipoMoneda, string direccion,
                                  string[] telefono, string web, string correo, string observaciones,
                                  String representanteLegal, String IDRepresentante, string codigo = "", Boolean activo = true) :
                                  base(tipoID, numeroId, nombre, TipoMoneda, direccion, telefono, web, correo, observaciones, codigo, activo)
        {
            this.representanteLegal = representanteLegal;
            this.IDRepresentante = IDRepresentante;
        }

        public String MyRepresentanteLegal
        {
            get { return representanteLegal; }
            set { representanteLegal = value; }
        }

        public String MyIDRepresentanteLegal
        {
            get { return IDRepresentante; }
            set { IDRepresentante = value; }
        }

    }
}
