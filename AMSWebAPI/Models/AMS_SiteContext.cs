using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace AMSWebAPI.Models
{
    public partial class AMS_SiteContext : DbContext
    {
        public AMS_SiteContext()
        {
        }

        public AMS_SiteContext(DbContextOptions<AMS_SiteContext> options)
            : base(options)
        {
        }

        private readonly string _connectionString;

        public AMS_SiteContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
            else if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=10.32.66.230\\SQLExpress2019;Database=AMS_Site;UID=sa;Password=Mistras1;");
            }
        }

        public void SetDatabase(HttpRequest request)
        {
            if (request.Headers.ContainsKey("DBName"))
            {
                string DBName = request.Headers["DBName"].ToString();
                if (!string.IsNullOrEmpty(DBName))
                {
                    SetDatabase(DBName);
                }
            }
        }

        public List<int> GetBufferSizes(HttpRequest request)
        {
            List<int> list = new List<int>();
            if (request.Headers.ContainsKey("BufferSize"))
            {
                string BufferSize = request.Headers["BufferSize"].ToString();
                if (!string.IsNullOrEmpty(BufferSize))
                {
                    list = BufferSize.Split(',').Select(Int32.Parse).ToList();
                }
            }
            return list;
        }

        public void SetDatabase(string DBName)
        {
            string connectionString = $"Server=10.32.66.230\\sqlexpress2019;Database={DBName};UID=sa;Password=Mistras1;MultipleActiveResultSets=true";
            this.Database.GetDbConnection().ConnectionString = connectionString;
        }

        public virtual DbSet<AmsDataAux> AmsDataAuxes { get; set; }
        public virtual DbSet<AmsDataBar> AmsDataBars { get; set; }
        public virtual DbSet<AmsDataBarFlags1> AmsDataBarFlags1s { get; set; }
        public virtual DbSet<AmsDataBarFlags2> AmsDataBarFlags2s { get; set; }
        public virtual DbSet<AmsDataBarFlags3> AmsDataBarFlags3s { get; set; }
        public virtual DbSet<AmsDataBarFlags4> AmsDataBarFlags4s { get; set; }
        public virtual DbSet<AmsDataBarFlags5> AmsDataBarFlags5s { get; set; }
        public virtual DbSet<AmsDataBarFlags6> AmsDataBarFlags6s { get; set; }
        public virtual DbSet<AmsDataBarFlags7> AmsDataBarFlags7s { get; set; }
        public virtual DbSet<AmsDataBarFlags8> AmsDataBarFlags8s { get; set; }
        public virtual DbSet<AmsDataDta1> AmsDataDta1s { get; set; }
        public virtual DbSet<AmsDataDta2> AmsDataDta2s { get; set; }
        public virtual DbSet<AmsDataDta3> AmsDataDta3s { get; set; }
        public virtual DbSet<AmsDataDta4> AmsDataDta4s { get; set; }
        public virtual DbSet<AmsDataDta5> AmsDataDta5s { get; set; }
        public virtual DbSet<AmsDataDta6> AmsDataDta6s { get; set; }
        public virtual DbSet<AmsDataDta7> AmsDataDta7s { get; set; }
        public virtual DbSet<AmsDataDta8> AmsDataDta8s { get; set; }
        public virtual DbSet<AmsDataDtatimestamp> AmsDataDtatimestamps { get; set; }
        public virtual DbSet<AmsDataRms1> AmsDataRms1s { get; set; }
        public virtual DbSet<AmsDataRms2> AmsDataRms2s { get; set; }
        public virtual DbSet<AmsDataRms3> AmsDataRms3s { get; set; }
        public virtual DbSet<AmsDataRms4> AmsDataRms4s { get; set; }
        public virtual DbSet<AmsDataRms5> AmsDataRms5s { get; set; }
        public virtual DbSet<AmsDataRms6> AmsDataRms6s { get; set; }
        public virtual DbSet<AmsDataRms7> AmsDataRms7s { get; set; }
        public virtual DbSet<AmsDataRms8> AmsDataRms8s { get; set; }
        public virtual DbSet<AmsDataRmsflags1> AmsDataRmsflags1s { get; set; }
        public virtual DbSet<AmsDataRmsflags2> AmsDataRmsflags2s { get; set; }
        public virtual DbSet<AmsDataRmsflags3> AmsDataRmsflags3s { get; set; }
        public virtual DbSet<AmsDataRmsflags4> AmsDataRmsflags4s { get; set; }
        public virtual DbSet<AmsDataRmsflags5> AmsDataRmsflags5s { get; set; }
        public virtual DbSet<AmsDataRmsflags6> AmsDataRmsflags6s { get; set; }
        public virtual DbSet<AmsDataRmsflags7> AmsDataRmsflags7s { get; set; }
        public virtual DbSet<AmsDataRmsflags8> AmsDataRmsflags8s { get; set; }
        public virtual DbSet<AmsDataRmstimestamp> AmsDataRmstimestamps { get; set; }
        public virtual DbSet<AmsFileLastUpdateUtc> AmsFileLastUpdateUtcs { get; set; }
        public virtual DbSet<AmsJournal1> AmsJournal1s { get; set; }
        public virtual DbSet<AmsJournal2> AmsJournal2s { get; set; }
        public virtual DbSet<AmsJournal3> AmsJournal3s { get; set; }
        public virtual DbSet<AmsJournal4> AmsJournal4s { get; set; }
        public virtual DbSet<AmsJournal5> AmsJournal5s { get; set; }
        public virtual DbSet<AmsJournal6> AmsJournal6s { get; set; }
        public virtual DbSet<AmsJournal7> AmsJournal7s { get; set; }
        public virtual DbSet<AmsJournal8> AmsJournal8s { get; set; }
        public virtual DbSet<AmsJournalTimestamp> AmsJournalTimestamps { get; set; }
        public virtual DbSet<AmsSensorLocBinary> AmsSensorLocBinaries { get; set; }
        public virtual DbSet<AmsSensorLocation> AmsSensorLocations { get; set; }
        public virtual DbSet<AmsSetting> AmsSettings { get; set; }
        public virtual DbSet<AmsSettingsString> AmsSettingsStrings { get; set; }
        public virtual DbSet<AmsSpecRefBinary> AmsSpecRefBinaries { get; set; }
        public virtual DbSet<AmsSpecRefChannelBinary> AmsSpecRefChannelBinaries { get; set; }
        public virtual DbSet<AmsStat> AmsStats { get; set; }
                
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AmsDataAux>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataAux");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsDataBar>(entity =>
            {
                entity.ToTable("AMS_DataBar");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsDataBarFlags1>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataBarFlags_1");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.Property(e => e.FC1).HasColumnName("fC1");

                entity.Property(e => e.FC10).HasColumnName("fC10");

                entity.Property(e => e.FC11).HasColumnName("fC11");

                entity.Property(e => e.FC12).HasColumnName("fC12");

                entity.Property(e => e.FC13).HasColumnName("fC13");

                entity.Property(e => e.FC14).HasColumnName("fC14");

                entity.Property(e => e.FC15).HasColumnName("fC15");

                entity.Property(e => e.FC16).HasColumnName("fC16");

                entity.Property(e => e.FC17).HasColumnName("fC17");

                entity.Property(e => e.FC18).HasColumnName("fC18");

                entity.Property(e => e.FC19).HasColumnName("fC19");

                entity.Property(e => e.FC2).HasColumnName("fC2");

                entity.Property(e => e.FC20).HasColumnName("fC20");

                entity.Property(e => e.FC21).HasColumnName("fC21");

                entity.Property(e => e.FC22).HasColumnName("fC22");

                entity.Property(e => e.FC23).HasColumnName("fC23");

                entity.Property(e => e.FC24).HasColumnName("fC24");

                entity.Property(e => e.FC3).HasColumnName("fC3");

                entity.Property(e => e.FC4).HasColumnName("fC4");

                entity.Property(e => e.FC5).HasColumnName("fC5");

                entity.Property(e => e.FC6).HasColumnName("fC6");

                entity.Property(e => e.FC7).HasColumnName("fC7");

                entity.Property(e => e.FC8).HasColumnName("fC8");

                entity.Property(e => e.FC9).HasColumnName("fC9");

                entity.Property(e => e.GC1).HasColumnName("gC1");

                entity.Property(e => e.GC10).HasColumnName("gC10");

                entity.Property(e => e.GC11).HasColumnName("gC11");

                entity.Property(e => e.GC12).HasColumnName("gC12");

                entity.Property(e => e.GC13).HasColumnName("gC13");

                entity.Property(e => e.GC14).HasColumnName("gC14");

                entity.Property(e => e.GC15).HasColumnName("gC15");

                entity.Property(e => e.GC16).HasColumnName("gC16");

                entity.Property(e => e.GC17).HasColumnName("gC17");

                entity.Property(e => e.GC18).HasColumnName("gC18");

                entity.Property(e => e.GC19).HasColumnName("gC19");

                entity.Property(e => e.GC2).HasColumnName("gC2");

                entity.Property(e => e.GC20).HasColumnName("gC20");

                entity.Property(e => e.GC21).HasColumnName("gC21");

                entity.Property(e => e.GC22).HasColumnName("gC22");

                entity.Property(e => e.GC23).HasColumnName("gC23");

                entity.Property(e => e.GC24).HasColumnName("gC24");

                entity.Property(e => e.GC3).HasColumnName("gC3");

                entity.Property(e => e.GC4).HasColumnName("gC4");

                entity.Property(e => e.GC5).HasColumnName("gC5");

                entity.Property(e => e.GC6).HasColumnName("gC6");

                entity.Property(e => e.GC7).HasColumnName("gC7");

                entity.Property(e => e.GC8).HasColumnName("gC8");

                entity.Property(e => e.GC9).HasColumnName("gC9");
            });

            modelBuilder.Entity<AmsDataBarFlags2>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataBarFlags_2");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.Property(e => e.FC25).HasColumnName("fC25");

                entity.Property(e => e.FC26).HasColumnName("fC26");

                entity.Property(e => e.FC27).HasColumnName("fC27");

                entity.Property(e => e.FC28).HasColumnName("fC28");

                entity.Property(e => e.FC29).HasColumnName("fC29");

                entity.Property(e => e.FC30).HasColumnName("fC30");

                entity.Property(e => e.FC31).HasColumnName("fC31");

                entity.Property(e => e.FC32).HasColumnName("fC32");

                entity.Property(e => e.FC33).HasColumnName("fC33");

                entity.Property(e => e.FC34).HasColumnName("fC34");

                entity.Property(e => e.FC35).HasColumnName("fC35");

                entity.Property(e => e.FC36).HasColumnName("fC36");

                entity.Property(e => e.FC37).HasColumnName("fC37");

                entity.Property(e => e.FC38).HasColumnName("fC38");

                entity.Property(e => e.FC39).HasColumnName("fC39");

                entity.Property(e => e.FC40).HasColumnName("fC40");

                entity.Property(e => e.FC41).HasColumnName("fC41");

                entity.Property(e => e.FC42).HasColumnName("fC42");

                entity.Property(e => e.FC43).HasColumnName("fC43");

                entity.Property(e => e.FC44).HasColumnName("fC44");

                entity.Property(e => e.FC45).HasColumnName("fC45");

                entity.Property(e => e.FC46).HasColumnName("fC46");

                entity.Property(e => e.FC47).HasColumnName("fC47");

                entity.Property(e => e.FC48).HasColumnName("fC48");

                entity.Property(e => e.GC25).HasColumnName("gC25");

                entity.Property(e => e.GC26).HasColumnName("gC26");

                entity.Property(e => e.GC27).HasColumnName("gC27");

                entity.Property(e => e.GC28).HasColumnName("gC28");

                entity.Property(e => e.GC29).HasColumnName("gC29");

                entity.Property(e => e.GC30).HasColumnName("gC30");

                entity.Property(e => e.GC31).HasColumnName("gC31");

                entity.Property(e => e.GC32).HasColumnName("gC32");

                entity.Property(e => e.GC33).HasColumnName("gC33");

                entity.Property(e => e.GC34).HasColumnName("gC34");

                entity.Property(e => e.GC35).HasColumnName("gC35");

                entity.Property(e => e.GC36).HasColumnName("gC36");

                entity.Property(e => e.GC37).HasColumnName("gC37");

                entity.Property(e => e.GC38).HasColumnName("gC38");

                entity.Property(e => e.GC39).HasColumnName("gC39");

                entity.Property(e => e.GC40).HasColumnName("gC40");

                entity.Property(e => e.GC41).HasColumnName("gC41");

                entity.Property(e => e.GC42).HasColumnName("gC42");

                entity.Property(e => e.GC43).HasColumnName("gC43");

                entity.Property(e => e.GC44).HasColumnName("gC44");

                entity.Property(e => e.GC45).HasColumnName("gC45");

                entity.Property(e => e.GC46).HasColumnName("gC46");

                entity.Property(e => e.GC47).HasColumnName("gC47");

                entity.Property(e => e.GC48).HasColumnName("gC48");
            });

            modelBuilder.Entity<AmsDataBarFlags3>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataBarFlags_3");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.Property(e => e.FC49).HasColumnName("fC49");

                entity.Property(e => e.FC50).HasColumnName("fC50");

                entity.Property(e => e.FC51).HasColumnName("fC51");

                entity.Property(e => e.FC52).HasColumnName("fC52");

                entity.Property(e => e.FC53).HasColumnName("fC53");

                entity.Property(e => e.FC54).HasColumnName("fC54");

                entity.Property(e => e.FC55).HasColumnName("fC55");

                entity.Property(e => e.FC56).HasColumnName("fC56");

                entity.Property(e => e.FC57).HasColumnName("fC57");

                entity.Property(e => e.FC58).HasColumnName("fC58");

                entity.Property(e => e.FC59).HasColumnName("fC59");

                entity.Property(e => e.FC60).HasColumnName("fC60");

                entity.Property(e => e.FC61).HasColumnName("fC61");

                entity.Property(e => e.FC62).HasColumnName("fC62");

                entity.Property(e => e.FC63).HasColumnName("fC63");

                entity.Property(e => e.FC64).HasColumnName("fC64");

                entity.Property(e => e.FC65).HasColumnName("fC65");

                entity.Property(e => e.FC66).HasColumnName("fC66");

                entity.Property(e => e.FC67).HasColumnName("fC67");

                entity.Property(e => e.FC68).HasColumnName("fC68");

                entity.Property(e => e.FC69).HasColumnName("fC69");

                entity.Property(e => e.FC70).HasColumnName("fC70");

                entity.Property(e => e.FC71).HasColumnName("fC71");

                entity.Property(e => e.FC72).HasColumnName("fC72");

                entity.Property(e => e.GC49).HasColumnName("gC49");

                entity.Property(e => e.GC50).HasColumnName("gC50");

                entity.Property(e => e.GC51).HasColumnName("gC51");

                entity.Property(e => e.GC52).HasColumnName("gC52");

                entity.Property(e => e.GC53).HasColumnName("gC53");

                entity.Property(e => e.GC54).HasColumnName("gC54");

                entity.Property(e => e.GC55).HasColumnName("gC55");

                entity.Property(e => e.GC56).HasColumnName("gC56");

                entity.Property(e => e.GC57).HasColumnName("gC57");

                entity.Property(e => e.GC58).HasColumnName("gC58");

                entity.Property(e => e.GC59).HasColumnName("gC59");

                entity.Property(e => e.GC60).HasColumnName("gC60");

                entity.Property(e => e.GC61).HasColumnName("gC61");

                entity.Property(e => e.GC62).HasColumnName("gC62");

                entity.Property(e => e.GC63).HasColumnName("gC63");

                entity.Property(e => e.GC64).HasColumnName("gC64");

                entity.Property(e => e.GC65).HasColumnName("gC65");

                entity.Property(e => e.GC66).HasColumnName("gC66");

                entity.Property(e => e.GC67).HasColumnName("gC67");

                entity.Property(e => e.GC68).HasColumnName("gC68");

                entity.Property(e => e.GC69).HasColumnName("gC69");

                entity.Property(e => e.GC70).HasColumnName("gC70");

                entity.Property(e => e.GC71).HasColumnName("gC71");

                entity.Property(e => e.GC72).HasColumnName("gC72");
            });

            modelBuilder.Entity<AmsDataBarFlags4>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataBarFlags_4");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.Property(e => e.FC73).HasColumnName("fC73");

                entity.Property(e => e.FC74).HasColumnName("fC74");

                entity.Property(e => e.FC75).HasColumnName("fC75");

                entity.Property(e => e.FC76).HasColumnName("fC76");

                entity.Property(e => e.FC77).HasColumnName("fC77");

                entity.Property(e => e.FC78).HasColumnName("fC78");

                entity.Property(e => e.FC79).HasColumnName("fC79");

                entity.Property(e => e.FC80).HasColumnName("fC80");

                entity.Property(e => e.FC81).HasColumnName("fC81");

                entity.Property(e => e.FC82).HasColumnName("fC82");

                entity.Property(e => e.FC83).HasColumnName("fC83");

                entity.Property(e => e.FC84).HasColumnName("fC84");

                entity.Property(e => e.FC85).HasColumnName("fC85");

                entity.Property(e => e.FC86).HasColumnName("fC86");

                entity.Property(e => e.FC87).HasColumnName("fC87");

                entity.Property(e => e.FC88).HasColumnName("fC88");

                entity.Property(e => e.FC89).HasColumnName("fC89");

                entity.Property(e => e.FC90).HasColumnName("fC90");

                entity.Property(e => e.FC91).HasColumnName("fC91");

                entity.Property(e => e.FC92).HasColumnName("fC92");

                entity.Property(e => e.FC93).HasColumnName("fC93");

                entity.Property(e => e.FC94).HasColumnName("fC94");

                entity.Property(e => e.FC95).HasColumnName("fC95");

                entity.Property(e => e.FC96).HasColumnName("fC96");

                entity.Property(e => e.GC73).HasColumnName("gC73");

                entity.Property(e => e.GC74).HasColumnName("gC74");

                entity.Property(e => e.GC75).HasColumnName("gC75");

                entity.Property(e => e.GC76).HasColumnName("gC76");

                entity.Property(e => e.GC77).HasColumnName("gC77");

                entity.Property(e => e.GC78).HasColumnName("gC78");

                entity.Property(e => e.GC79).HasColumnName("gC79");

                entity.Property(e => e.GC80).HasColumnName("gC80");

                entity.Property(e => e.GC81).HasColumnName("gC81");

                entity.Property(e => e.GC82).HasColumnName("gC82");

                entity.Property(e => e.GC83).HasColumnName("gC83");

                entity.Property(e => e.GC84).HasColumnName("gC84");

                entity.Property(e => e.GC85).HasColumnName("gC85");

                entity.Property(e => e.GC86).HasColumnName("gC86");

                entity.Property(e => e.GC87).HasColumnName("gC87");

                entity.Property(e => e.GC88).HasColumnName("gC88");

                entity.Property(e => e.GC89).HasColumnName("gC89");

                entity.Property(e => e.GC90).HasColumnName("gC90");

                entity.Property(e => e.GC91).HasColumnName("gC91");

                entity.Property(e => e.GC92).HasColumnName("gC92");

                entity.Property(e => e.GC93).HasColumnName("gC93");

                entity.Property(e => e.GC94).HasColumnName("gC94");

                entity.Property(e => e.GC95).HasColumnName("gC95");

                entity.Property(e => e.GC96).HasColumnName("gC96");
            });

            modelBuilder.Entity<AmsDataBarFlags5>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataBarFlags_5");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.Property(e => e.FC100).HasColumnName("fC100");

                entity.Property(e => e.FC101).HasColumnName("fC101");

                entity.Property(e => e.FC102).HasColumnName("fC102");

                entity.Property(e => e.FC103).HasColumnName("fC103");

                entity.Property(e => e.FC104).HasColumnName("fC104");

                entity.Property(e => e.FC105).HasColumnName("fC105");

                entity.Property(e => e.FC106).HasColumnName("fC106");

                entity.Property(e => e.FC107).HasColumnName("fC107");

                entity.Property(e => e.FC108).HasColumnName("fC108");

                entity.Property(e => e.FC109).HasColumnName("fC109");

                entity.Property(e => e.FC110).HasColumnName("fC110");

                entity.Property(e => e.FC111).HasColumnName("fC111");

                entity.Property(e => e.FC112).HasColumnName("fC112");

                entity.Property(e => e.FC113).HasColumnName("fC113");

                entity.Property(e => e.FC114).HasColumnName("fC114");

                entity.Property(e => e.FC115).HasColumnName("fC115");

                entity.Property(e => e.FC116).HasColumnName("fC116");

                entity.Property(e => e.FC117).HasColumnName("fC117");

                entity.Property(e => e.FC118).HasColumnName("fC118");

                entity.Property(e => e.FC119).HasColumnName("fC119");

                entity.Property(e => e.FC120).HasColumnName("fC120");

                entity.Property(e => e.FC97).HasColumnName("fC97");

                entity.Property(e => e.FC98).HasColumnName("fC98");

                entity.Property(e => e.FC99).HasColumnName("fC99");

                entity.Property(e => e.GC100).HasColumnName("gC100");

                entity.Property(e => e.GC101).HasColumnName("gC101");

                entity.Property(e => e.GC102).HasColumnName("gC102");

                entity.Property(e => e.GC103).HasColumnName("gC103");

                entity.Property(e => e.GC104).HasColumnName("gC104");

                entity.Property(e => e.GC105).HasColumnName("gC105");

                entity.Property(e => e.GC106).HasColumnName("gC106");

                entity.Property(e => e.GC107).HasColumnName("gC107");

                entity.Property(e => e.GC108).HasColumnName("gC108");

                entity.Property(e => e.GC109).HasColumnName("gC109");

                entity.Property(e => e.GC110).HasColumnName("gC110");

                entity.Property(e => e.GC111).HasColumnName("gC111");

                entity.Property(e => e.GC112).HasColumnName("gC112");

                entity.Property(e => e.GC113).HasColumnName("gC113");

                entity.Property(e => e.GC114).HasColumnName("gC114");

                entity.Property(e => e.GC115).HasColumnName("gC115");

                entity.Property(e => e.GC116).HasColumnName("gC116");

                entity.Property(e => e.GC117).HasColumnName("gC117");

                entity.Property(e => e.GC118).HasColumnName("gC118");

                entity.Property(e => e.GC119).HasColumnName("gC119");

                entity.Property(e => e.GC120).HasColumnName("gC120");

                entity.Property(e => e.GC97).HasColumnName("gC97");

                entity.Property(e => e.GC98).HasColumnName("gC98");

                entity.Property(e => e.GC99).HasColumnName("gC99");
            });

            modelBuilder.Entity<AmsDataBarFlags6>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataBarFlags_6");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.Property(e => e.FC121).HasColumnName("fC121");

                entity.Property(e => e.FC122).HasColumnName("fC122");

                entity.Property(e => e.FC123).HasColumnName("fC123");

                entity.Property(e => e.FC124).HasColumnName("fC124");

                entity.Property(e => e.FC125).HasColumnName("fC125");

                entity.Property(e => e.FC126).HasColumnName("fC126");

                entity.Property(e => e.FC127).HasColumnName("fC127");

                entity.Property(e => e.FC128).HasColumnName("fC128");

                entity.Property(e => e.FC129).HasColumnName("fC129");

                entity.Property(e => e.FC130).HasColumnName("fC130");

                entity.Property(e => e.FC131).HasColumnName("fC131");

                entity.Property(e => e.FC132).HasColumnName("fC132");

                entity.Property(e => e.FC133).HasColumnName("fC133");

                entity.Property(e => e.FC134).HasColumnName("fC134");

                entity.Property(e => e.FC135).HasColumnName("fC135");

                entity.Property(e => e.FC136).HasColumnName("fC136");

                entity.Property(e => e.FC137).HasColumnName("fC137");

                entity.Property(e => e.FC138).HasColumnName("fC138");

                entity.Property(e => e.FC139).HasColumnName("fC139");

                entity.Property(e => e.FC140).HasColumnName("fC140");

                entity.Property(e => e.FC141).HasColumnName("fC141");

                entity.Property(e => e.FC142).HasColumnName("fC142");

                entity.Property(e => e.FC143).HasColumnName("fC143");

                entity.Property(e => e.FC144).HasColumnName("fC144");

                entity.Property(e => e.GC121).HasColumnName("gC121");

                entity.Property(e => e.GC122).HasColumnName("gC122");

                entity.Property(e => e.GC123).HasColumnName("gC123");

                entity.Property(e => e.GC124).HasColumnName("gC124");

                entity.Property(e => e.GC125).HasColumnName("gC125");

                entity.Property(e => e.GC126).HasColumnName("gC126");

                entity.Property(e => e.GC127).HasColumnName("gC127");

                entity.Property(e => e.GC128).HasColumnName("gC128");

                entity.Property(e => e.GC129).HasColumnName("gC129");

                entity.Property(e => e.GC130).HasColumnName("gC130");

                entity.Property(e => e.GC131).HasColumnName("gC131");

                entity.Property(e => e.GC132).HasColumnName("gC132");

                entity.Property(e => e.GC133).HasColumnName("gC133");

                entity.Property(e => e.GC134).HasColumnName("gC134");

                entity.Property(e => e.GC135).HasColumnName("gC135");

                entity.Property(e => e.GC136).HasColumnName("gC136");

                entity.Property(e => e.GC137).HasColumnName("gC137");

                entity.Property(e => e.GC138).HasColumnName("gC138");

                entity.Property(e => e.GC139).HasColumnName("gC139");

                entity.Property(e => e.GC140).HasColumnName("gC140");

                entity.Property(e => e.GC141).HasColumnName("gC141");

                entity.Property(e => e.GC142).HasColumnName("gC142");

                entity.Property(e => e.GC143).HasColumnName("gC143");

                entity.Property(e => e.GC144).HasColumnName("gC144");
            });

            modelBuilder.Entity<AmsDataBarFlags7>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataBarFlags_7");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.Property(e => e.FC145).HasColumnName("fC145");

                entity.Property(e => e.FC146).HasColumnName("fC146");

                entity.Property(e => e.FC147).HasColumnName("fC147");

                entity.Property(e => e.FC148).HasColumnName("fC148");

                entity.Property(e => e.FC149).HasColumnName("fC149");

                entity.Property(e => e.FC150).HasColumnName("fC150");

                entity.Property(e => e.FC151).HasColumnName("fC151");

                entity.Property(e => e.FC152).HasColumnName("fC152");

                entity.Property(e => e.FC153).HasColumnName("fC153");

                entity.Property(e => e.FC154).HasColumnName("fC154");

                entity.Property(e => e.FC155).HasColumnName("fC155");

                entity.Property(e => e.FC156).HasColumnName("fC156");

                entity.Property(e => e.FC157).HasColumnName("fC157");

                entity.Property(e => e.FC158).HasColumnName("fC158");

                entity.Property(e => e.FC159).HasColumnName("fC159");

                entity.Property(e => e.FC160).HasColumnName("fC160");

                entity.Property(e => e.FC161).HasColumnName("fC161");

                entity.Property(e => e.FC162).HasColumnName("fC162");

                entity.Property(e => e.FC163).HasColumnName("fC163");

                entity.Property(e => e.FC164).HasColumnName("fC164");

                entity.Property(e => e.FC165).HasColumnName("fC165");

                entity.Property(e => e.FC166).HasColumnName("fC166");

                entity.Property(e => e.FC167).HasColumnName("fC167");

                entity.Property(e => e.FC168).HasColumnName("fC168");

                entity.Property(e => e.GC145).HasColumnName("gC145");

                entity.Property(e => e.GC146).HasColumnName("gC146");

                entity.Property(e => e.GC147).HasColumnName("gC147");

                entity.Property(e => e.GC148).HasColumnName("gC148");

                entity.Property(e => e.GC149).HasColumnName("gC149");

                entity.Property(e => e.GC150).HasColumnName("gC150");

                entity.Property(e => e.GC151).HasColumnName("gC151");

                entity.Property(e => e.GC152).HasColumnName("gC152");

                entity.Property(e => e.GC153).HasColumnName("gC153");

                entity.Property(e => e.GC154).HasColumnName("gC154");

                entity.Property(e => e.GC155).HasColumnName("gC155");

                entity.Property(e => e.GC156).HasColumnName("gC156");

                entity.Property(e => e.GC157).HasColumnName("gC157");

                entity.Property(e => e.GC158).HasColumnName("gC158");

                entity.Property(e => e.GC159).HasColumnName("gC159");

                entity.Property(e => e.GC160).HasColumnName("gC160");

                entity.Property(e => e.GC161).HasColumnName("gC161");

                entity.Property(e => e.GC162).HasColumnName("gC162");

                entity.Property(e => e.GC163).HasColumnName("gC163");

                entity.Property(e => e.GC164).HasColumnName("gC164");

                entity.Property(e => e.GC165).HasColumnName("gC165");

                entity.Property(e => e.GC166).HasColumnName("gC166");

                entity.Property(e => e.GC167).HasColumnName("gC167");

                entity.Property(e => e.GC168).HasColumnName("gC168");
            });

            modelBuilder.Entity<AmsDataBarFlags8>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataBarFlags_8");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.Property(e => e.FC169).HasColumnName("fC169");

                entity.Property(e => e.FC170).HasColumnName("fC170");

                entity.Property(e => e.FC171).HasColumnName("fC171");

                entity.Property(e => e.FC172).HasColumnName("fC172");

                entity.Property(e => e.FC173).HasColumnName("fC173");

                entity.Property(e => e.FC174).HasColumnName("fC174");

                entity.Property(e => e.FC175).HasColumnName("fC175");

                entity.Property(e => e.FC176).HasColumnName("fC176");

                entity.Property(e => e.FC177).HasColumnName("fC177");

                entity.Property(e => e.FC178).HasColumnName("fC178");

                entity.Property(e => e.FC179).HasColumnName("fC179");

                entity.Property(e => e.FC180).HasColumnName("fC180");

                entity.Property(e => e.FC181).HasColumnName("fC181");

                entity.Property(e => e.FC182).HasColumnName("fC182");

                entity.Property(e => e.FC183).HasColumnName("fC183");

                entity.Property(e => e.FC184).HasColumnName("fC184");

                entity.Property(e => e.FC185).HasColumnName("fC185");

                entity.Property(e => e.FC186).HasColumnName("fC186");

                entity.Property(e => e.FC187).HasColumnName("fC187");

                entity.Property(e => e.FC188).HasColumnName("fC188");

                entity.Property(e => e.FC189).HasColumnName("fC189");

                entity.Property(e => e.FC190).HasColumnName("fC190");

                entity.Property(e => e.FC191).HasColumnName("fC191");

                entity.Property(e => e.FC192).HasColumnName("fC192");

                entity.Property(e => e.GC169).HasColumnName("gC169");

                entity.Property(e => e.GC170).HasColumnName("gC170");

                entity.Property(e => e.GC171).HasColumnName("gC171");

                entity.Property(e => e.GC172).HasColumnName("gC172");

                entity.Property(e => e.GC173).HasColumnName("gC173");

                entity.Property(e => e.GC174).HasColumnName("gC174");

                entity.Property(e => e.GC175).HasColumnName("gC175");

                entity.Property(e => e.GC176).HasColumnName("gC176");

                entity.Property(e => e.GC177).HasColumnName("gC177");

                entity.Property(e => e.GC178).HasColumnName("gC178");

                entity.Property(e => e.GC179).HasColumnName("gC179");

                entity.Property(e => e.GC180).HasColumnName("gC180");

                entity.Property(e => e.GC181).HasColumnName("gC181");

                entity.Property(e => e.GC182).HasColumnName("gC182");

                entity.Property(e => e.GC183).HasColumnName("gC183");

                entity.Property(e => e.GC184).HasColumnName("gC184");

                entity.Property(e => e.GC185).HasColumnName("gC185");

                entity.Property(e => e.GC186).HasColumnName("gC186");

                entity.Property(e => e.GC187).HasColumnName("gC187");

                entity.Property(e => e.GC188).HasColumnName("gC188");

                entity.Property(e => e.GC189).HasColumnName("gC189");

                entity.Property(e => e.GC190).HasColumnName("gC190");

                entity.Property(e => e.GC191).HasColumnName("gC191");

                entity.Property(e => e.GC192).HasColumnName("gC192");
            });

            modelBuilder.Entity<AmsDataDta1>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataDTA_1");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.Property(e => e.C1).HasMaxLength(512);

                entity.Property(e => e.C10).HasMaxLength(512);

                entity.Property(e => e.C11).HasMaxLength(512);

                entity.Property(e => e.C12).HasMaxLength(512);

                entity.Property(e => e.C13).HasMaxLength(512);

                entity.Property(e => e.C14).HasMaxLength(512);

                entity.Property(e => e.C15).HasMaxLength(512);

                entity.Property(e => e.C16).HasMaxLength(512);

                entity.Property(e => e.C17).HasMaxLength(512);

                entity.Property(e => e.C18).HasMaxLength(512);

                entity.Property(e => e.C19).HasMaxLength(512);

                entity.Property(e => e.C2).HasMaxLength(512);

                entity.Property(e => e.C20).HasMaxLength(512);

                entity.Property(e => e.C21).HasMaxLength(512);

                entity.Property(e => e.C22).HasMaxLength(512);

                entity.Property(e => e.C23).HasMaxLength(512);

                entity.Property(e => e.C24).HasMaxLength(512);

                entity.Property(e => e.C3).HasMaxLength(512);

                entity.Property(e => e.C4).HasMaxLength(512);

                entity.Property(e => e.C5).HasMaxLength(512);

                entity.Property(e => e.C6).HasMaxLength(512);

                entity.Property(e => e.C7).HasMaxLength(512);

                entity.Property(e => e.C8).HasMaxLength(512);

                entity.Property(e => e.C9).HasMaxLength(512);
            });

            modelBuilder.Entity<AmsDataDta2>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataDTA_2");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.Property(e => e.C25).HasMaxLength(512);

                entity.Property(e => e.C26).HasMaxLength(512);

                entity.Property(e => e.C27).HasMaxLength(512);

                entity.Property(e => e.C28).HasMaxLength(512);

                entity.Property(e => e.C29).HasMaxLength(512);

                entity.Property(e => e.C30).HasMaxLength(512);

                entity.Property(e => e.C31).HasMaxLength(512);

                entity.Property(e => e.C32).HasMaxLength(512);

                entity.Property(e => e.C33).HasMaxLength(512);

                entity.Property(e => e.C34).HasMaxLength(512);

                entity.Property(e => e.C35).HasMaxLength(512);

                entity.Property(e => e.C36).HasMaxLength(512);

                entity.Property(e => e.C37).HasMaxLength(512);

                entity.Property(e => e.C38).HasMaxLength(512);

                entity.Property(e => e.C39).HasMaxLength(512);

                entity.Property(e => e.C40).HasMaxLength(512);

                entity.Property(e => e.C41).HasMaxLength(512);

                entity.Property(e => e.C42).HasMaxLength(512);

                entity.Property(e => e.C43).HasMaxLength(512);

                entity.Property(e => e.C44).HasMaxLength(512);

                entity.Property(e => e.C45).HasMaxLength(512);

                entity.Property(e => e.C46).HasMaxLength(512);

                entity.Property(e => e.C47).HasMaxLength(512);

                entity.Property(e => e.C48).HasMaxLength(512);
            });

            modelBuilder.Entity<AmsDataDta3>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataDTA_3");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.Property(e => e.C49).HasMaxLength(512);

                entity.Property(e => e.C50).HasMaxLength(512);

                entity.Property(e => e.C51).HasMaxLength(512);

                entity.Property(e => e.C52).HasMaxLength(512);

                entity.Property(e => e.C53).HasMaxLength(512);

                entity.Property(e => e.C54).HasMaxLength(512);

                entity.Property(e => e.C55).HasMaxLength(512);

                entity.Property(e => e.C56).HasMaxLength(512);

                entity.Property(e => e.C57).HasMaxLength(512);

                entity.Property(e => e.C58).HasMaxLength(512);

                entity.Property(e => e.C59).HasMaxLength(512);

                entity.Property(e => e.C60).HasMaxLength(512);

                entity.Property(e => e.C61).HasMaxLength(512);

                entity.Property(e => e.C62).HasMaxLength(512);

                entity.Property(e => e.C63).HasMaxLength(512);

                entity.Property(e => e.C64).HasMaxLength(512);

                entity.Property(e => e.C65).HasMaxLength(512);

                entity.Property(e => e.C66).HasMaxLength(512);

                entity.Property(e => e.C67).HasMaxLength(512);

                entity.Property(e => e.C68).HasMaxLength(512);

                entity.Property(e => e.C69).HasMaxLength(512);

                entity.Property(e => e.C70).HasMaxLength(512);

                entity.Property(e => e.C71).HasMaxLength(512);

                entity.Property(e => e.C72).HasMaxLength(512);
            });

            modelBuilder.Entity<AmsDataDta4>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataDTA_4");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.Property(e => e.C73).HasMaxLength(512);

                entity.Property(e => e.C74).HasMaxLength(512);

                entity.Property(e => e.C75).HasMaxLength(512);

                entity.Property(e => e.C76).HasMaxLength(512);

                entity.Property(e => e.C77).HasMaxLength(512);

                entity.Property(e => e.C78).HasMaxLength(512);

                entity.Property(e => e.C79).HasMaxLength(512);

                entity.Property(e => e.C80).HasMaxLength(512);

                entity.Property(e => e.C81).HasMaxLength(512);

                entity.Property(e => e.C82).HasMaxLength(512);

                entity.Property(e => e.C83).HasMaxLength(512);

                entity.Property(e => e.C84).HasMaxLength(512);

                entity.Property(e => e.C85).HasMaxLength(512);

                entity.Property(e => e.C86).HasMaxLength(512);

                entity.Property(e => e.C87).HasMaxLength(512);

                entity.Property(e => e.C88).HasMaxLength(512);

                entity.Property(e => e.C89).HasMaxLength(512);

                entity.Property(e => e.C90).HasMaxLength(512);

                entity.Property(e => e.C91).HasMaxLength(512);

                entity.Property(e => e.C92).HasMaxLength(512);

                entity.Property(e => e.C93).HasMaxLength(512);

                entity.Property(e => e.C94).HasMaxLength(512);

                entity.Property(e => e.C95).HasMaxLength(512);

                entity.Property(e => e.C96).HasMaxLength(512);
            });

            modelBuilder.Entity<AmsDataDta5>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataDTA_5");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.Property(e => e.C100).HasMaxLength(512);

                entity.Property(e => e.C101).HasMaxLength(512);

                entity.Property(e => e.C102).HasMaxLength(512);

                entity.Property(e => e.C103).HasMaxLength(512);

                entity.Property(e => e.C104).HasMaxLength(512);

                entity.Property(e => e.C105).HasMaxLength(512);

                entity.Property(e => e.C106).HasMaxLength(512);

                entity.Property(e => e.C107).HasMaxLength(512);

                entity.Property(e => e.C108).HasMaxLength(512);

                entity.Property(e => e.C109).HasMaxLength(512);

                entity.Property(e => e.C110).HasMaxLength(512);

                entity.Property(e => e.C111).HasMaxLength(512);

                entity.Property(e => e.C112).HasMaxLength(512);

                entity.Property(e => e.C113).HasMaxLength(512);

                entity.Property(e => e.C114).HasMaxLength(512);

                entity.Property(e => e.C115).HasMaxLength(512);

                entity.Property(e => e.C116).HasMaxLength(512);

                entity.Property(e => e.C117).HasMaxLength(512);

                entity.Property(e => e.C118).HasMaxLength(512);

                entity.Property(e => e.C119).HasMaxLength(512);

                entity.Property(e => e.C120).HasMaxLength(512);

                entity.Property(e => e.C97).HasMaxLength(512);

                entity.Property(e => e.C98).HasMaxLength(512);

                entity.Property(e => e.C99).HasMaxLength(512);
            });

            modelBuilder.Entity<AmsDataDta6>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataDTA_6");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.Property(e => e.C121).HasMaxLength(512);

                entity.Property(e => e.C122).HasMaxLength(512);

                entity.Property(e => e.C123).HasMaxLength(512);

                entity.Property(e => e.C124).HasMaxLength(512);

                entity.Property(e => e.C125).HasMaxLength(512);

                entity.Property(e => e.C126).HasMaxLength(512);

                entity.Property(e => e.C127).HasMaxLength(512);

                entity.Property(e => e.C128).HasMaxLength(512);

                entity.Property(e => e.C129).HasMaxLength(512);

                entity.Property(e => e.C130).HasMaxLength(512);

                entity.Property(e => e.C131).HasMaxLength(512);

                entity.Property(e => e.C132).HasMaxLength(512);

                entity.Property(e => e.C133).HasMaxLength(512);

                entity.Property(e => e.C134).HasMaxLength(512);

                entity.Property(e => e.C135).HasMaxLength(512);

                entity.Property(e => e.C136).HasMaxLength(512);

                entity.Property(e => e.C137).HasMaxLength(512);

                entity.Property(e => e.C138).HasMaxLength(512);

                entity.Property(e => e.C139).HasMaxLength(512);

                entity.Property(e => e.C140).HasMaxLength(512);

                entity.Property(e => e.C141).HasMaxLength(512);

                entity.Property(e => e.C142).HasMaxLength(512);

                entity.Property(e => e.C143).HasMaxLength(512);

                entity.Property(e => e.C144).HasMaxLength(512);
            });

            modelBuilder.Entity<AmsDataDta7>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataDTA_7");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.Property(e => e.C145).HasMaxLength(512);

                entity.Property(e => e.C146).HasMaxLength(512);

                entity.Property(e => e.C147).HasMaxLength(512);

                entity.Property(e => e.C148).HasMaxLength(512);

                entity.Property(e => e.C149).HasMaxLength(512);

                entity.Property(e => e.C150).HasMaxLength(512);

                entity.Property(e => e.C151).HasMaxLength(512);

                entity.Property(e => e.C152).HasMaxLength(512);

                entity.Property(e => e.C153).HasMaxLength(512);

                entity.Property(e => e.C154).HasMaxLength(512);

                entity.Property(e => e.C155).HasMaxLength(512);

                entity.Property(e => e.C156).HasMaxLength(512);

                entity.Property(e => e.C157).HasMaxLength(512);

                entity.Property(e => e.C158).HasMaxLength(512);

                entity.Property(e => e.C159).HasMaxLength(512);

                entity.Property(e => e.C160).HasMaxLength(512);

                entity.Property(e => e.C161).HasMaxLength(512);

                entity.Property(e => e.C162).HasMaxLength(512);

                entity.Property(e => e.C163).HasMaxLength(512);

                entity.Property(e => e.C164).HasMaxLength(512);

                entity.Property(e => e.C165).HasMaxLength(512);

                entity.Property(e => e.C166).HasMaxLength(512);

                entity.Property(e => e.C167).HasMaxLength(512);

                entity.Property(e => e.C168).HasMaxLength(512);
            });

            modelBuilder.Entity<AmsDataDta8>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataDTA_8");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.Property(e => e.C169).HasMaxLength(512);

                entity.Property(e => e.C170).HasMaxLength(512);

                entity.Property(e => e.C171).HasMaxLength(512);

                entity.Property(e => e.C172).HasMaxLength(512);

                entity.Property(e => e.C173).HasMaxLength(512);

                entity.Property(e => e.C174).HasMaxLength(512);

                entity.Property(e => e.C175).HasMaxLength(512);

                entity.Property(e => e.C176).HasMaxLength(512);

                entity.Property(e => e.C177).HasMaxLength(512);

                entity.Property(e => e.C178).HasMaxLength(512);

                entity.Property(e => e.C179).HasMaxLength(512);

                entity.Property(e => e.C180).HasMaxLength(512);

                entity.Property(e => e.C181).HasMaxLength(512);

                entity.Property(e => e.C182).HasMaxLength(512);

                entity.Property(e => e.C183).HasMaxLength(512);

                entity.Property(e => e.C184).HasMaxLength(512);

                entity.Property(e => e.C185).HasMaxLength(512);

                entity.Property(e => e.C186).HasMaxLength(512);

                entity.Property(e => e.C187).HasMaxLength(512);

                entity.Property(e => e.C188).HasMaxLength(512);

                entity.Property(e => e.C189).HasMaxLength(512);

                entity.Property(e => e.C190).HasMaxLength(512);

                entity.Property(e => e.C191).HasMaxLength(512);

                entity.Property(e => e.C192).HasMaxLength(512);
            });

            modelBuilder.Entity<AmsDataDtatimestamp>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataDTATimestamp");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsDataRms1>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataRMS_1");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsDataRms2>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataRMS_2");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsDataRms3>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataRMS_3");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsDataRms4>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataRMS_4");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsDataRms5>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataRMS_5");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsDataRms6>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataRMS_6");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsDataRms7>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataRMS_7");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsDataRms8>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataRMS_8");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsDataRmsflags1>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataRMSFlags_1");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.HasOne(d => d.UtcNavigation)
                    .WithOne(p => p.AmsDataRmsflags1)
                    .HasForeignKey<AmsDataRmsflags1>(d => d.Utc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AMS_DataRMSFlags_1_AMS_DataRMS_1");
            });

            modelBuilder.Entity<AmsDataRmsflags2>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataRMSFlags_2");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.HasOne(d => d.UtcNavigation)
                    .WithOne(p => p.AmsDataRmsflags2)
                    .HasForeignKey<AmsDataRmsflags2>(d => d.Utc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AMS_DataRMSFlags_2_AMS_DataRMS_2");
            });

            modelBuilder.Entity<AmsDataRmsflags3>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataRMSFlags_3");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.HasOne(d => d.UtcNavigation)
                    .WithOne(p => p.AmsDataRmsflags3)
                    .HasForeignKey<AmsDataRmsflags3>(d => d.Utc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AMS_DataRMSFlags_3_AMS_DataRMS_3");
            });

            modelBuilder.Entity<AmsDataRmsflags4>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataRMSFlags_4");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.HasOne(d => d.UtcNavigation)
                    .WithOne(p => p.AmsDataRmsflags4)
                    .HasForeignKey<AmsDataRmsflags4>(d => d.Utc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AMS_DataRMSFlags_4_AMS_DataRMS_4");
            });

            modelBuilder.Entity<AmsDataRmsflags5>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataRMSFlags_5");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.HasOne(d => d.UtcNavigation)
                    .WithOne(p => p.AmsDataRmsflags5)
                    .HasForeignKey<AmsDataRmsflags5>(d => d.Utc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AMS_DataRMSFlags_5_AMS_DataRMS_5");
            });

            modelBuilder.Entity<AmsDataRmsflags6>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataRMSFlags_6");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.HasOne(d => d.UtcNavigation)
                    .WithOne(p => p.AmsDataRmsflags6)
                    .HasForeignKey<AmsDataRmsflags6>(d => d.Utc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AMS_DataRMSFlags_6_AMS_DataRMS_6");
            });

            modelBuilder.Entity<AmsDataRmsflags7>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataRMSFlags_7");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.HasOne(d => d.UtcNavigation)
                    .WithOne(p => p.AmsDataRmsflags7)
                    .HasForeignKey<AmsDataRmsflags7>(d => d.Utc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AMS_DataRMSFlags_7_AMS_DataRMS_7");
            });

            modelBuilder.Entity<AmsDataRmsflags8>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataRMSFlags_8");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.HasOne(d => d.UtcNavigation)
                    .WithOne(p => p.AmsDataRmsflags8)
                    .HasForeignKey<AmsDataRmsflags8>(d => d.Utc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AMS_DataRMSFlags_8_AMS_DataRMS_8");
            });

            modelBuilder.Entity<AmsDataRmstimestamp>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_DataRMSTimestamp");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsFileLastUpdateUtc>(entity =>
            {
                entity.HasKey(e => e.FileName);

                entity.ToTable("AMS_FileLastUpdateUTC");

                entity.Property(e => e.FileName).HasMaxLength(50);

                entity.Property(e => e.LastModifiedUtc)
                    .HasColumnType("datetime")
                    .HasColumnName("LastModifiedUTC");
            });

            modelBuilder.Entity<AmsJournal1>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_Journal_1");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsJournal2>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_Journal_2");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsJournal3>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_Journal_3");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsJournal4>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_Journal_4");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsJournal5>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_Journal_5");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsJournal6>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_Journal_6");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsJournal7>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_Journal_7");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsJournal8>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_Journal_8");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsJournalTimestamp>(entity =>
            {
                entity.HasKey(e => e.Utc);

                entity.ToTable("AMS_JournalTimestamp");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");
            });

            modelBuilder.Entity<AmsSensorLocBinary>(entity =>
            {
                entity.HasKey(e => e.Utc)
                    .HasName("PK_AMS_SensorBinary_1");

                entity.ToTable("AMS_SensorLocBinary");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.Property(e => e.SensorLoc).IsRequired();
            });

            modelBuilder.Entity<AmsSensorLocation>(entity =>
            {
                entity.HasKey(e => new { e.Utc, e.SensorId })
                    .HasName("PK_AMS_SensorLoccation");

                entity.ToTable("AMS_SensorLocation");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.Property(e => e.SensorId).HasColumnName("SensorID");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.XLoc).HasColumnName("xLoc");

                entity.Property(e => e.YLoc).HasColumnName("yLoc");

                entity.HasOne(d => d.UtcNavigation)
                    .WithMany(p => p.AmsSensorLocations)
                    .HasForeignKey(d => d.Utc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AMS_SensorLoccation_AMS_SensorBinary");
            });

            modelBuilder.Entity<AmsSetting>(entity =>
            {
                entity.ToTable("AMS_Settings");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LastUpdateUtc)
                    .HasColumnType("datetime")
                    .HasColumnName("LastUpdateUTC");

                entity.Property(e => e.Settings).IsRequired();
            });

            modelBuilder.Entity<AmsSettingsString>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("AMS_SettingsString");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.LastUpdateUtc)
                    .HasColumnType("datetime")
                    .HasColumnName("LastUpdateUTC");

                entity.Property(e => e.Settings).IsRequired();
            });

            modelBuilder.Entity<AmsSpecRefBinary>(entity =>
            {
                entity.HasKey(e => e.Utc)
                    .HasName("PK_AMS_DataSpecRef");

                entity.ToTable("AMS_SpecRefBinary");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.Property(e => e.SpecRef).IsRequired();
            });

            modelBuilder.Entity<AmsSpecRefChannelBinary>(entity =>
            {
                entity.HasKey(e => new { e.Utc, e.CId });

                entity.ToTable("AMS_SpecRef_Channel_Binary");

                entity.Property(e => e.Utc)
                    .HasColumnType("datetime")
                    .HasColumnName("UTC");

                entity.Property(e => e.CId).HasColumnName("C_ID");

                entity.Property(e => e.SpecRef).HasMaxLength(512);

                entity.HasOne(d => d.UtcNavigation)
                    .WithMany(p => p.AmsSpecRefChannelBinaries)
                    .HasForeignKey(d => d.Utc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AMS_DataSpecRef_CID_SpecRef_AMS_DataSpecRef");
            });

            modelBuilder.Entity<AmsStat>(entity =>
            {
                entity.HasKey(e => e.ChannelIdx);

                entity.ToTable("AMS_Stat");

                entity.Property(e => e.ChannelIdx).ValueGeneratedNever();

                entity.Property(e => e.Afbidx).HasColumnName("AFBIdx");

                entity.Property(e => e.Afbjid).HasColumnName("AFBJID");

                entity.Property(e => e.Afbtemp).HasColumnName("AFBTemp");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
