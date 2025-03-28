namespace webtest
{
    public enum Tipovi
    {
        Single,
        EP,
        LP,
        Album

    }
    public class Ploca
    {
        public int id { get; set; }
        public string naziv { get; set; } 
        public string izvodjac { get; set; }
        public string zanr { get; set; }
        public Tipovi tip { get; set; }
        public int trajanje {  get; set; }

        public Ploca(int id,string naziv,string izvodjac,string zanr,Tipovi tip,int trajanje)
        {   
            this.id = id;
            this.naziv = naziv;
            this.izvodjac= izvodjac;
            this.zanr = zanr;
            this.tip = tip;
            this.trajanje = trajanje;
        }


    }
}
