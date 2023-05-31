namespace IExpro.Core.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("INVOICE")]
    public partial class INVOICE
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InvoiceId { get; set; }

        public int? PackingId { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public int? consignorId { get; set; }

        public int? cosigneeId { get; set; }

        public int? GriNo { get; set; }

        public int? BillToCustomer { get; set; }

        public double? InvoiceAmount { get; set; }

        [StringLength(500)]
        public string Contents { get; set; }

        [StringLength(50)]
        public string Knots { get; set; }

        public int? DelTerms { get; set; }

        public int? Terms { get; set; }

        public int? CreditId { get; set; }

        public int? ShipingId { get; set; }

        public int? FrmBlDate { get; set; }

        [StringLength(50)]
        public string LcNo { get; set; }

        public int? PreCarrier { get; set; }

        public int? Receipt { get; set; }

        [StringLength(50)]
        public string VesselName { get; set; }

        public double? NetWt { get; set; }

        public double? GrossWt { get; set; }

        [StringLength(50)]
        public string RollMark { get; set; }

        public int? NoOfRolls { get; set; }

        [StringLength(50)]
        public string Consignee { get; set; }

        [StringLength(50)]
        public string BLNo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? BlDt { get; set; }

        public double? Freight { get; set; }

        public double? Insurance { get; set; }

        public double? InvSubTotal { get; set; }

        public int? AgentId { get; set; }

        [StringLength(50)]
        public string GSPNo { get; set; }

        [StringLength(50)]
        public string InsPolicyNo { get; set; }

        [StringLength(50)]
        public string BankRefNo { get; set; }

        [StringLength(50)]
        public string BankRefDt { get; set; }

        public int? BillPurchase { get; set; }

        [StringLength(50)]
        public string PostShipAdv { get; set; }

        [StringLength(50)]
        public string ValueDt { get; set; }

        public double? AmtRealised { get; set; }

        public double? AmtPlusMinus { get; set; }

        public int? PortLoad { get; set; }

        [StringLength(50)]
        public string PortUnload { get; set; }

        [StringLength(50)]
        public string DestinationAdd { get; set; }

        public double? LessAdvance { get; set; }

        public double? Commisionpercent { get; set; }

        public double? CommisionAmount { get; set; }

        public int? goodsId { get; set; }

        public int? TypeOFBuyerOtherConsignee { get; set; }

        public int? UserId { get; set; }

        public double? ComPer { get; set; }

        public DateTime? duedate { get; set; }

        public DateTime? Paydate { get; set; }

        public int? currencytype { get; set; }

        public double? Ex1Rate { get; set; }

        public double? BillAmount { get; set; }

        public double? Ex2Rate { get; set; }

        public int? Id { get; set; }

        [StringLength(100)]
        public string ERef { get; set; }

        [StringLength(50)]
        public string EXControlNo { get; set; }

        [StringLength(50)]
        public string ExControlDate { get; set; }

        [StringLength(30)]
        public string TInvoiceNo { get; set; }

        [StringLength(30)]
        public string TPackingNo { get; set; }

        public int? frmbldateafter { get; set; }

        public int? DrawbackPaid { get; set; }

        [StringLength(50)]
        public string sbillno { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? sbilldate { get; set; }

        public double? Drawbackamount { get; set; }

        [StringLength(50)]
        public string SDFNo { get; set; }

        [StringLength(50)]
        public string GRNo { get; set; }

        [StringLength(100)]
        public string DrawBackTerms { get; set; }

        [StringLength(100)]
        public string SingleCountryPerson { get; set; }

        public double? DiscountAmt { get; set; }

        [StringLength(100)]
        public string FreightTerms { get; set; }

        [StringLength(100)]
        public string OtherRef { get; set; }

        [StringLength(500)]
        public string TConsignor { get; set; }

        [StringLength(500)]
        public string TConsignee { get; set; }

        [StringLength(500)]
        public string TBuyerOConsignee { get; set; }

        [StringLength(50)]
        public string CompMark { get; set; }

        [StringLength(50)]
        public string CustmMark { get; set; }

        [StringLength(500)]
        public string SpclInstr { get; set; }

        [StringLength(15)]
        public string Lcdate { get; set; }

        public double? ExchRate { get; set; }

        [StringLength(50)]
        public string TRRRNOAndDate { get; set; }

        [StringLength(50)]
        public string DocType { get; set; }

        [StringLength(100)]
        public string DocRef { get; set; }

        [StringLength(500)]
        public string NOTIFY { get; set; }

        [StringLength(50)]
        public string ACDPERSON { get; set; }

        public int? InvoiceType { get; set; }

        [StringLength(100)]
        public string RollMarkHead { get; set; }

        [StringLength(200)]
        public string ExtraChargeRemarks { get; set; }

        public double? ExtraCharges { get; set; }

        [StringLength(500)]
        public string BLAgencyInfo { get; set; }

        [StringLength(500)]
        public string BLAgent { get; set; }

        public int? PcsWiseFlag { get; set; }

        public int? Status { get; set; }

        [StringLength(500)]
        public string Declaration { get; set; }

        public int? RecAmtFlag { get; set; }

        public int? YardFlag { get; set; }

        [StringLength(50)]
        public string OrderNo { get; set; }

        [StringLength(300)]
        public string InvoiceCovering { get; set; }

        [StringLength(300)]
        public string ProformaRef { get; set; }

        public double? brcExchRate { get; set; }

        public int? InvoiceYear { get; set; }

        [StringLength(1000)]
        public string TorderNo { get; set; }

        [StringLength(20)]
        public string FIRCNo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FIRCNoDate { get; set; }

        [StringLength(20)]
        public string FormANo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FormADate { get; set; }

        [StringLength(20)]
        public string OpenPolicyNo { get; set; }

        [StringLength(20)]
        public string CreditNo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreditNoDate { get; set; }

        [StringLength(20)]
        public string PInvoiceNo { get; set; }

        [StringLength(100)]
        public string Package { get; set; }

        [StringLength(50)]
        public string Container { get; set; }

        [StringLength(20)]
        public string ContainerNo { get; set; }

        [StringLength(20)]
        public string SealNo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? PInvoiceDate { get; set; }

        public int? BankId { get; set; }

        public double? Agent_Commission { get; set; }

        public double? License_Commission { get; set; }

        public double? CBM { get; set; }

        public double? UnitRate { get; set; }

        public DateTime? BankSubmissionDate { get; set; }

        [StringLength(50)]
        public string BankSubmissionRefNo { get; set; }

        [StringLength(50)]
        public string Dtstamp { get; set; }

        [StringLength(50)]
        public string Delvwk { get; set; }

        public DateTime? Dispatchdate { get; set; }

        public double? grossamount { get; set; }

        [StringLength(1000)]
        public string descriptionofgoods { get; set; }

        [StringLength(50)]
        public string countryoforigin { get; set; }

        [StringLength(50)]
        public string countryoffinaldest { get; set; }

        public DateTime? BOEDate { get; set; }

        [StringLength(100)]
        public string shipmentid { get; set; }

        [StringLength(50)]
        public string truckno { get; set; }

        [StringLength(50)]
        public string Destcode { get; set; }

        public double? volume { get; set; }

        public double? CGST { get; set; }

        public double? SGST { get; set; }

        public double? IGST { get; set; }

        public double? INRRate { get; set; }

        [StringLength(50)]
        public string LUTARNNO { get; set; }

        [StringLength(500)]
        public string ShipToAddress { get; set; }

        [StringLength(250)]
        public string DiscountRemark { get; set; }

        [StringLength(50)]
        public string GstinType { get; set; }

        [StringLength(50)]
        public string EndUse { get; set; }

        [StringLength(250)]
        public string RawMaterialPile { get; set; }

        [StringLength(250)]
        public string RawMaterialBase { get; set; }

        public int? PaymentTermCustom { get; set; }

        [StringLength(30)]
        public string CountryOfFinalDestination { get; set; }

        [StringLength(30)]
        public string PlaceOfDelivery { get; set; }

        [StringLength(200)]
        public string WareHouseName { get; set; }

        public int? ExportAgainstIGST { get; set; }

        public int? ExportAgainstLUT { get; set; }

        [StringLength(20)]
        public string SUQty { get; set; }

        [StringLength(20)]
        public string PreferentialAgreement { get; set; }

        public double? PackingCharges { get; set; }

        [StringLength(30)]
        public string FlightNo { get; set; }

        public DateTime? FlightDate { get; set; }

        public DateTime? LeoDate { get; set; }

        [StringLength(500)]
        public string Composition { get; set; }
    }
}
