using System;
using System.Collections.Generic;
using JAPAN.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace JAPAN.Data;

public partial class JapanContext : DbContext
{
    public JapanContext()
    {
    }

    public JapanContext(DbContextOptions<JapanContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ForumOdgovor> ForumOdgovori { get; set; }

    public virtual DbSet<ForumPitanje> ForumPitanja { get; set; }

    public virtual DbSet<Ispit> Ispiti { get; set; }

    public virtual DbSet<Korisnik> Korisnici { get; set; }

    public virtual DbSet<Odgovor> Odgovori { get; set; }

    public virtual DbSet<Pitanje> Pitanja { get; set; }

    public virtual DbSet<Statistika> Statistike { get; set; }

    public virtual DbSet<Tecaj> Tecaji { get; set; }

    public virtual DbSet<Tezina> Tezine { get; set; }

    public virtual DbSet<TipSadrzaj> TipoviSadrzaja { get; set; }

    public virtual DbSet<Uloga> Uloge { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ForumOdgovor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("forum_odgovor_pkey");

            entity.ToTable("forum_odgovor");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Idkorisnik).HasColumnName("idkorisnik");
            entity.Property(e => e.Idpitanje).HasColumnName("idpitanje");
            entity.Property(e => e.Kreirano).HasColumnName("kreirano");
            entity.Property(e => e.Sadrzaj)
                .HasMaxLength(1000)
                .HasColumnName("sadrzaj");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.ForumOdgovori)
                .HasForeignKey(d => d.Idkorisnik)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("forum_odgovor_idkorisnik_fkey");

            entity.HasOne(d => d.ForumPitanje).WithMany(p => p.ForumOdgovori)
                .HasForeignKey(d => d.Idpitanje)
                .HasConstraintName("forum_odgovor_idpitanje_fkey");
        });

        modelBuilder.Entity<ForumPitanje>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("forum_pitanje_pkey");

            entity.ToTable("forum_pitanje");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Idkorisnik).HasColumnName("idkorisnik");
            entity.Property(e => e.Kreirano).HasColumnName("kreirano");
            entity.Property(e => e.Naslov)
                .HasMaxLength(100)
                .HasColumnName("naslov");
            entity.Property(e => e.Sadrzaj)
                .HasMaxLength(1000)
                .HasColumnName("sadrzaj");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.ForumPitanja)
                .HasForeignKey(d => d.Idkorisnik)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("forum_pitanje_idkorisnik_fkey");
        });

        modelBuilder.Entity<Ispit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ispit_pkey");

            entity.ToTable("ispit");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Idtezina).HasColumnName("idtezina");
            entity.Property(e => e.Naziv)
                .HasMaxLength(50)
                .HasColumnName("naziv");
            entity.Property(e => e.Opis)
                .HasMaxLength(100)
                .HasColumnName("opis");
            entity.Property(e => e.Pozicija).HasColumnName("pozicija");

            entity.HasOne(d => d.Tezina).WithMany(p => p.Ispiti)
                .HasForeignKey(d => d.Idtezina)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ispit_idtezina_fkey");

            entity.HasMany(d => d.Pitanja).WithMany(p => p.Ispiti)
                .UsingEntity<Dictionary<string, object>>(
                    "Ispitpitanje",
                    r => r.HasOne<Pitanje>().WithMany()
                        .HasForeignKey("Idpitanje")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("ispitpitanje_idpitanje_fkey"),
                    l => l.HasOne<Ispit>().WithMany()
                        .HasForeignKey("Idispit")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("ispitpitanje_idispit_fkey"),
                    j =>
                    {
                        j.HasKey("Idispit", "Idpitanje").HasName("ispitpitanje_pkey");
                        j.ToTable("ispitpitanje");
                        j.IndexerProperty<int>("Idispit").HasColumnName("idispit");
                        j.IndexerProperty<int>("Idpitanje").HasColumnName("idpitanje");
                    });
        });

        modelBuilder.Entity<Korisnik>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("korisnik_pkey");

            entity.ToTable("korisnik");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Identifikator)
                .HasMaxLength(50)
                .HasColumnName("identifikator");
            entity.Property(e => e.Iduloga).HasColumnName("iduloga");
            entity.Property(e => e.Korisnickoime)
                .HasMaxLength(50)
                .HasColumnName("korisnickoime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Ime)
                .HasMaxLength(50)
                .HasColumnName("ime");
            entity.Property(e => e.Prezime)
                .HasMaxLength(50)
                .HasColumnName("prezime");
            entity.Property(e => e.DatumRodenja).HasColumnName("datumrodenja");
            entity.Property(e => e.Preporuka)
                .HasMaxLength(50)
                .HasColumnName("preporuka");

            entity.HasIndex(e => e.Korisnickoime).IsUnique();
            entity.HasIndex(e => e.Id).IsUnique();

            entity.HasOne(d => d.Uloga).WithMany(p => p.Korisnici)
                .HasForeignKey(d => d.Iduloga)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("korisnik_iduloga_fkey");
        });

        modelBuilder.Entity<Odgovor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("odgovor_pkey");

            entity.ToTable("odgovor");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Idpitanje).HasColumnName("idpitanje");
            entity.Property(e => e.Tekst)
                .HasMaxLength(100)
                .HasColumnName("tekst");
            entity.Property(e => e.Tocno).HasColumnName("tocno");

            entity.HasOne(d => d.Pitanje).WithMany(p => p.Odgovori)
                .HasForeignKey(d => d.Idpitanje)
                .HasConstraintName("odgovor_idpitanje_fkey");
        });

        modelBuilder.Entity<Pitanje>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pitanje_pkey");

            entity.ToTable("pitanje");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Tekst)
                .HasMaxLength(100)
                .HasColumnName("tekst");
        });

        modelBuilder.Entity<Statistika>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("statistika_pkey");

            entity.ToTable("statistika");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Idispit).HasColumnName("idispit");
            entity.Property(e => e.Idkorisnik).HasColumnName("idkorisnik");
            entity.Property(e => e.Idtecaj).HasColumnName("idtecaj");
            entity.Property(e => e.Rezultat)
                .HasMaxLength(20)
                .HasColumnName("rezultat");
            entity.Property(e => e.Zavrseno).HasColumnName("zavrseno");

            entity.HasOne(d => d.Ispit).WithMany(p => p.Statistike)
                .HasForeignKey(d => d.Idispit)
                .HasConstraintName("statistika_idispit_fkey");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Statistike)
                .HasForeignKey(d => d.Idkorisnik)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("statistika_idkorisnik_fkey");

            entity.HasOne(d => d.Tecaj).WithMany(p => p.Statistike)
                .HasForeignKey(d => d.Idtecaj)
                .HasConstraintName("statistika_idtecaj_fkey");
        });

        modelBuilder.Entity<Tecaj>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tecaj_pkey");

            entity.ToTable("tecaj");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Idtezina).HasColumnName("idtezina");
            entity.Property(e => e.Idtipsadrzaj).HasColumnName("idtipsadrzaj");
            entity.Property(e => e.Kreirano).HasColumnName("kreirano");
            entity.Property(e => e.Naziv)
                .HasMaxLength(50)
                .HasColumnName("naziv");
            entity.Property(e => e.Opis)
                .HasMaxLength(100)
                .HasColumnName("opis");
            entity.Property(e => e.Pozicija).HasColumnName("pozicija");
            entity.Property(e => e.Sadrzaj)
                .HasMaxLength(1000)
                .HasColumnName("sadrzaj");

            entity.HasOne(d => d.Tezina).WithMany(p => p.Tecaji)
                .HasForeignKey(d => d.Idtezina)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tecaj_idtezina_fkey");

            entity.HasOne(d => d.Tipsadrzaja).WithMany(p => p.Tecaji)
                .HasForeignKey(d => d.Idtipsadrzaj)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tecaj_idtipsadrzaj_fkey");
        });

        modelBuilder.Entity<Tezina>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tezina_pkey");

            entity.ToTable("tezina");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Naziv)
                .HasMaxLength(20)
                .HasColumnName("naziv");
        });

        modelBuilder.Entity<TipSadrzaj>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tip_sadrzaj_pkey");

            entity.ToTable("tip_sadrzaj");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Naziv)
                .HasMaxLength(20)
                .HasColumnName("naziv");
        });

        modelBuilder.Entity<Uloga>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("uloga_pkey");

            entity.ToTable("uloga");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Naziv)
                .HasMaxLength(20)
                .HasColumnName("naziv");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
