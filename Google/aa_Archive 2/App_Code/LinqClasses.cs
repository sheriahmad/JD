using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;



    [Table(Name="dbo.ecom_GlobalAdmin")]
    public class ECOM_GlobalAdmin
    {

        private int _pkID = 0;
        private string _changeEmailTo;

   
        [Column(Name="pkID", Storage = "_pkID", DbType = "Int NOT NULL IDENTITY",
        IsPrimaryKey = true, IsDbGenerated = true)]
        public int pkID
        {
            get { return this._pkID; }
            // No need to specify a setter because IsDBGenerated is
            // true.
        }

        [Column(Name = "approvalEmailTo")]
        public String approvalEmailTo { get; set; }


        [Column(Name="changeEmailTo", Storage = "_changeEmailTo", DbType = "varchar(50)")]
        public string changeEmailTo
        {
            get { return this._changeEmailTo; }
            set { this._changeEmailTo = value; }
        }

        [Column(Name = "newAccountEmailFrom")]
        public String newAccountAmailFrom { get; set; }

    }

