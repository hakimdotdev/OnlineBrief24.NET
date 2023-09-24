using System.Numerics;
using System;

namespace OnlineBrief24.Models
{
    public class Parameters
    {
        public Guid Id { get; set; }
		///<summary>
		///<para>0 = Druck s/w</para>
		///<para>1 = Druck farbig</para>
		/// </summary>
		public bool Colored { get; set; }
		///<summary>
		///<para>0 = Druck einseitig(simplex)</para>
		///<para>1 = Druck doppelseitig(duplex)</para>
		///</summary>
		public bool Mode { get; set; }
		/// <summary>
		///<para>0 = DIN lang-Kuvert</para>
		///<para>1 = C4-Kuvert</para>
		/// </summary>
		public bool Envelope { get; set; }
		/// <summary>
		///<para>0 = Automatische Versandzonenwahl</para>
		///<para>1 = National(Deutschland)</para>
		///<para>2 = Nicht belegt(ehemals Europa)</para>
		///<para>3 = International(ehemals Welt)</para>
		/// </summary>
		public int ShippingZone { get; set; }
		/// <summary>
		///<para>0 = Kein Einschreiben</para>
		///<para>1 = Einschreiben Einwurf</para>
		///<para>2 = Einschreiben Standard</para>
		///<para>3 = Einschreiben Eigenhändig</para>
		/// </summary>
		public int RegisteredMail { get; set; }
		/// <summary>
		///<para>0 = Kein Zahlschein</para>
		///<para>1 = Nicht belegt(ehemals Inlands-Zahlschei)</para>
		///<para>2 = SEPA-Zahlschein</para>
		/// </summary>
		public int PaymentSlip { get; set; }

		public string Reserve { get; set; } = "0000000";
		public virtual ICollection<File> Files { get; set; }
		/// <summary>
		/// Ruft den ersten Teil des Dateinamens ab, welcher sich aus den Parametern ergibt.
		/// </summary>
		/// <returns></returns>
        public string GetFileNamePart()
        {
            string druck = this.Colored ? "1" : "0";
            string modus = this.Mode ? "1" : "0";
            string kuvert = this.Envelope ? "1" : "0";
            string versandZone = this.ShippingZone.ToString();
            string einschreiben = this.RegisteredMail.ToString();
            string zahlschein = this.PaymentSlip.ToString();
            string reserve = this.Reserve;
            return druck+modus+kuvert+versandZone+einschreiben+zahlschein+reserve;
        }
    }
}
