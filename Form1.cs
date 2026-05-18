using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace OtoPazarProjesi
{
    public partial class Form1 : Form
    {
        OtoPazarEntities db = new OtoPazarEntities();
        int aktifKullaniciId = 0;
        string aktifKullaniciRol = "";
        Urun aktifIncelenenUrun = null; 

        Panel pnlGiris, pnlKayit, pnlGirisKutusu;
        Panel pnlSatici, pnlAlici, pnlIlanDetay;
        
        TextBox txtKadi, txtSifre, txtYeniKadi, txtYeniSifre;
        ComboBox cmbRol;
        Button btnGiris, btnKayit;

        FlowLayoutPanel flpSatici;
        TextBox txtUrunBaslik, txtUrunFiyat, txtUrunKm, txtUrunSaseNo, txtUrunModel;
        ComboBox cmbUrunDurum;
        PictureBox picSecilenResim;
        Button btnResimSec, btnIlanEkle, btnCikisSatici;
        Image secilenResimReferansi = null;
        DataGridView dgvSaticiMesajlar; 

        FlowLayoutPanel flpAlici;
        TextBox txtAra;
        Button btnCikisAlici;
        DataGridView dgvAliciMesajlar; 

        PictureBox picDetay;
        Label lblDetayBaslik, lblDetayFiyat, lblDetayBilgi;
        TextBox txtDetayMesaj;
        Button btnDetayMesajGonder, btnDetayGeri;

        public Form1()
        {
            InitializeComponent();
            ArayuzuOlustur();
            BaslangicVerileriEkle();
        }

        private void BaslangicVerileriEkle()
        {
            db.Kullanicilar.Add(new Kullanici { Id = 1, KullaniciAdi = "alici_demo", Sifre = "1234", Rol = "Alici" });
            db.Kullanicilar.Add(new Kullanici { Id = 2, KullaniciAdi = "satici_demo", Sifre = "1234", Rol = "Satici" });
            
            Image imgMercedes = ResimOlustur("Mercedes Parça");
            Image imgAudi = ResimOlustur("Audi Parça");

            db.Urunler.Add(new Urun { Id = 1, Baslik = "Mercedes Benz Sibop Kapağı", Fiyat = 300, SaticiId = 2, Tarih = DateTime.Now, UrunResmi = imgMercedes, Durum = "Sıfır", Km = 0, SaseNo = "WDB2100351A654321", ModelYili = 2018 });
            db.Urunler.Add(new Urun { Id = 2, Baslik = "Audi sport kaporta", Fiyat = 90000, SaticiId = 2, Tarih = DateTime.Now, UrunResmi = imgAudi, Durum = "İkinci El", Km = 0, SaseNo = "WDB56400351D654321", ModelYili = 2017 });
            
            db.Mesajlar.Add(new Mesaj { Id = 1, GonderenId = 1, AliciId = 2, Icerik = "Kaporta için son fiyat ne olur? İndirim yapar mısınız?", Tarih = DateTime.Now });
        }

        private Image ResimOlustur(string yazi)
        {
            Bitmap b = new Bitmap(200, 150); 
            using(Graphics g = Graphics.FromImage(b)) { 
                g.Clear(Color.Gray); 
                g.DrawString(yazi, new Font("Segoe UI", 16), Brushes.White, 10, 60); 
            }
            return b;
        }

        private void ArayuzuOlustur()
        {
            this.Text = "Oto Pazar & Araç Parça Otomasyonu";
            this.WindowState = FormWindowState.Maximized; 
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 11);

            pnlGiris = new Panel { Dock = DockStyle.Fill, Visible = true };
            
            pnlKayit = new Panel { Size = new Size(420, 520), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            pnlKayit.Controls.Add(new Label { Text = "Sisteme Kayıt Ol", Location = new Point(40, 40), Font = new Font("Segoe UI", 24, FontStyle.Bold), AutoSize=true, ForeColor = Color.DarkSlateBlue });
            
            pnlKayit.Controls.Add(new Label { Text = "Kullanıcı Adı:", Location = new Point(40, 130), AutoSize=true, Font = new Font("Segoe UI", 13) });
            txtYeniKadi = new TextBox { Location = new Point(40, 160), Width=340, Font = new Font("Segoe UI", 14) }; pnlKayit.Controls.Add(txtYeniKadi);
            
            pnlKayit.Controls.Add(new Label { Text = "Şifre:", Location = new Point(40, 210), AutoSize=true, Font = new Font("Segoe UI", 13) });
            txtYeniSifre = new TextBox { Location = new Point(40, 240), Width=340, Font = new Font("Segoe UI", 14), PasswordChar = '*' }; pnlKayit.Controls.Add(txtYeniSifre);
            
            pnlKayit.Controls.Add(new Label { Text = "Hesap Türü:", Location = new Point(40, 290), AutoSize=true, Font = new Font("Segoe UI", 13) });
            cmbRol = new ComboBox { Location = new Point(40, 320), Width=340, Font = new Font("Segoe UI", 14), DropDownStyle=ComboBoxStyle.DropDownList }; 
            cmbRol.Items.AddRange(new string[] { "Alici", "Satici" }); cmbRol.SelectedIndex = 0; pnlKayit.Controls.Add(cmbRol);
            
            btnKayit = new Button { Text = "Kayıt Ol", Location = new Point(40, 410), Width=340, Height=60, BackColor = Color.DarkSlateBlue, ForeColor = Color.White, Font = new Font("Segoe UI", 14, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnKayit.Click += BtnKayit_Click; pnlKayit.Controls.Add(btnKayit);
            pnlGiris.Controls.Add(pnlKayit);

            pnlGirisKutusu = new Panel { Size = new Size(420, 520), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            pnlGirisKutusu.Controls.Add(new Label { Text = "Giriş Yap", Location = new Point(40, 40), Font = new Font("Segoe UI", 24, FontStyle.Bold), AutoSize=true, ForeColor = Color.SeaGreen });
            
            pnlGirisKutusu.Controls.Add(new Label { Text = "Kullanıcı Adı:", Location = new Point(40, 130), AutoSize=true, Font = new Font("Segoe UI", 13) });
            txtKadi = new TextBox { Location = new Point(40, 160), Width=340, Font = new Font("Segoe UI", 14) }; pnlGirisKutusu.Controls.Add(txtKadi);
            
            pnlGirisKutusu.Controls.Add(new Label { Text = "Şifre:", Location = new Point(40, 210), AutoSize=true, Font = new Font("Segoe UI", 13) });
            txtSifre = new TextBox { Location = new Point(40, 240), Width=340, Font = new Font("Segoe UI", 14), PasswordChar = '*' }; pnlGirisKutusu.Controls.Add(txtSifre);
            
            btnGiris = new Button { Text = "Giriş Yap", Location = new Point(40, 410), Width=340, Height=60, BackColor = Color.SeaGreen, ForeColor = Color.White, Font = new Font("Segoe UI", 14, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnGiris.Click += BtnGiris_Click; pnlGirisKutusu.Controls.Add(btnGiris);
            pnlGiris.Controls.Add(pnlGirisKutusu);

            pnlGiris.Resize += (s, e) => {
                int totalWidth = 940; 
                int startX = (pnlGiris.Width - totalWidth) / 2;
                int y = (pnlGiris.Height - 520) / 2;
                pnlKayit.Location = new Point(startX > 0 ? startX : 50, y > 0 ? y : 50);
                pnlGirisKutusu.Location = new Point((startX > 0 ? startX : 50) + 520, y > 0 ? y : 50);
            };
            this.Controls.Add(pnlGiris);

            pnlSatici = new Panel { Dock = DockStyle.Fill, Visible = false, BackColor = Color.White };
            Panel pnlSaticiUst = new Panel { Dock = DockStyle.Top, Height = 90, BackColor = Color.DarkSlateBlue };
            pnlSaticiUst.Controls.Add(new Label { Text = "Satıcı Paneli - İlanlarım", Location = new Point(30, 25), Font = new Font("Segoe UI", 22, FontStyle.Bold), AutoSize=true, ForeColor = Color.White });
            btnCikisSatici = new Button { Text = "Güvenli Çıkış", Anchor = AnchorStyles.Right | AnchorStyles.Top, Location = new Point(pnlSaticiUst.Width - 160, 25), Width=140, Height=45, BackColor=Color.LightCoral, Font = new Font("Segoe UI", 11, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand }; btnCikisSatici.Click += BtnCikis_Click; pnlSaticiUst.Controls.Add(btnCikisSatici);
            pnlSatici.Controls.Add(pnlSaticiUst);

            Panel pnlSaticiSag = new Panel { Dock = DockStyle.Right, Width = 450, BackColor = Color.LightSteelBlue, BorderStyle = BorderStyle.FixedSingle };
            pnlSaticiSag.Controls.Add(new Label { Text = "📬 Gelen Mesajlar", Location = new Point(15, 15), Font = new Font("Segoe UI", 18, FontStyle.Bold), AutoSize=true });
            pnlSaticiSag.Controls.Add(new Label { Text = "(Mesajı yanıtlamak için üzerine tıklayın)", Location = new Point(15, 50), Font = new Font("Segoe UI", 11, FontStyle.Italic), AutoSize=true, ForeColor = Color.DimGray });
            dgvSaticiMesajlar = new DataGridView { Location = new Point(15, 80), Size = new Size(415, 600), SelectionMode = DataGridViewSelectionMode.FullRowSelect, AllowUserToAddRows=false, ReadOnly=true, AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill, AutoSizeRowsMode=DataGridViewAutoSizeRowsMode.AllCells, DefaultCellStyle = new DataGridViewCellStyle { WrapMode = DataGridViewTriState.True, Font=new Font("Segoe UI", 12) }, ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Font=new Font("Segoe UI", 12, FontStyle.Bold) }, RowTemplate = { MinimumHeight = 40 } }; 
            dgvSaticiMesajlar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvSaticiMesajlar.CellClick += DgvSaticiMesajlar_CellClick;
            pnlSaticiSag.Controls.Add(dgvSaticiMesajlar);
            pnlSatici.Controls.Add(pnlSaticiSag);

            Panel pnlIlanEkle = new Panel { Dock = DockStyle.Left, Width = 380, BackColor = Color.WhiteSmoke, BorderStyle = BorderStyle.FixedSingle };
            pnlIlanEkle.Controls.Add(new Label { Text = "Yeni Parça/Araç Ekle", Location = new Point(20, 20), Font = new Font("Segoe UI", 18, FontStyle.Bold), AutoSize=true, ForeColor = Color.Teal });
            
            pnlIlanEkle.Controls.Add(new Label { Text = "Başlık:", Location = new Point(20, 80), AutoSize=true, Font = new Font("Segoe UI", 12, FontStyle.Bold) });
            txtUrunBaslik = new TextBox { Location = new Point(20, 110), Width=330, Font = new Font("Segoe UI", 13) }; pnlIlanEkle.Controls.Add(txtUrunBaslik);
            
            pnlIlanEkle.Controls.Add(new Label { Text = "Fiyat (TL):", Location = new Point(20, 160), AutoSize=true, Font = new Font("Segoe UI", 12, FontStyle.Bold) });
            txtUrunFiyat = new TextBox { Location = new Point(20, 190), Width=150, Font = new Font("Segoe UI", 13) }; pnlIlanEkle.Controls.Add(txtUrunFiyat);

            pnlIlanEkle.Controls.Add(new Label { Text = "Yıl/Model:", Location = new Point(200, 160), AutoSize=true, Font = new Font("Segoe UI", 12, FontStyle.Bold) });
            txtUrunModel = new TextBox { Location = new Point(200, 190), Width=150, Font = new Font("Segoe UI", 13) }; pnlIlanEkle.Controls.Add(txtUrunModel);

            pnlIlanEkle.Controls.Add(new Label { Text = "Durum:", Location = new Point(20, 240), AutoSize=true, Font = new Font("Segoe UI", 12, FontStyle.Bold) });
            cmbUrunDurum = new ComboBox { Location = new Point(20, 270), Width=150, Font = new Font("Segoe UI", 13), DropDownStyle=ComboBoxStyle.DropDownList };
            cmbUrunDurum.Items.AddRange(new string[] { "Sıfır", "İkinci El" }); cmbUrunDurum.SelectedIndex = 0; pnlIlanEkle.Controls.Add(cmbUrunDurum);

            pnlIlanEkle.Controls.Add(new Label { Text = "KM:", Location = new Point(200, 240), AutoSize=true, Font = new Font("Segoe UI", 12, FontStyle.Bold) });
            txtUrunKm = new TextBox { Location = new Point(200, 270), Width=150, Font = new Font("Segoe UI", 13) }; pnlIlanEkle.Controls.Add(txtUrunKm);

            pnlIlanEkle.Controls.Add(new Label { Text = "Şase No / Parça Kodu:", Location = new Point(20, 320), AutoSize=true, Font = new Font("Segoe UI", 12, FontStyle.Bold) });
            txtUrunSaseNo = new TextBox { Location = new Point(20, 350), Width=330, Font = new Font("Segoe UI", 13) }; pnlIlanEkle.Controls.Add(txtUrunSaseNo);

            pnlIlanEkle.Controls.Add(new Label { Text = "Görsel:", Location = new Point(20, 400), AutoSize=true, Font = new Font("Segoe UI", 12, FontStyle.Bold) });
            picSecilenResim = new PictureBox { Location = new Point(20, 430), Size = new Size(180, 130), BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.LightGray }; pnlIlanEkle.Controls.Add(picSecilenResim);
            btnResimSec = new Button { Text = "Resim Seç\n(Bilgisayardan)", Location = new Point(220, 430), Width=130, Height=130, BackColor = Color.DarkOrange, ForeColor = Color.White, Font = new Font("Segoe UI", 11, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand }; btnResimSec.Click += BtnResimSec_Click; pnlIlanEkle.Controls.Add(btnResimSec);

            btnIlanEkle = new Button { Text = "✔ İlanı Hemen Yayınla", Location = new Point(20, 590), Width=330, Height=60, BackColor = Color.SeaGreen, ForeColor = Color.White, Font = new Font("Segoe UI", 16, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand }; btnIlanEkle.Click += BtnIlanEkle_Click; pnlIlanEkle.Controls.Add(btnIlanEkle);
            pnlSatici.Controls.Add(pnlIlanEkle);

            flpSatici = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(30) };
            pnlSatici.Controls.Add(flpSatici);
            this.Controls.Add(pnlSatici);

            pnlAlici = new Panel { Dock = DockStyle.Fill, Visible = false, BackColor = Color.White };
            Panel pnlAliciUst = new Panel { Dock = DockStyle.Top, Height = 90, BackColor = Color.SeaGreen };
            pnlAliciUst.Controls.Add(new Label { Text = "Tüm İlanlar", Location = new Point(30, 25), Font = new Font("Segoe UI", 22, FontStyle.Bold), AutoSize=true, ForeColor = Color.White });
            
            pnlAliciUst.Controls.Add(new Label { Text = "🔍 Arama:", Location = new Point(450, 32), Font = new Font("Segoe UI", 16, FontStyle.Bold), AutoSize=true, ForeColor = Color.White });
            txtAra = new TextBox { Location = new Point(580, 30), Width=300, Font = new Font("Segoe UI", 16) };
            txtAra.TextChanged += TxtAra_TextChanged; pnlAliciUst.Controls.Add(txtAra);

            btnCikisAlici = new Button { Text = "Güvenli Çıkış", Anchor = AnchorStyles.Right | AnchorStyles.Top, Location = new Point(pnlAliciUst.Width - 160, 25), Width=140, Height=45, BackColor=Color.LightCoral, Font = new Font("Segoe UI", 11, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand }; btnCikisAlici.Click += BtnCikis_Click; pnlAliciUst.Controls.Add(btnCikisAlici);
            pnlAlici.Controls.Add(pnlAliciUst);

            Panel pnlAliciSag = new Panel { Dock = DockStyle.Right, Width = 450, BackColor = Color.LightSteelBlue, BorderStyle = BorderStyle.FixedSingle };
            pnlAliciSag.Controls.Add(new Label { Text = "📬 Gelen Mesajlar", Location = new Point(15, 15), Font = new Font("Segoe UI", 18, FontStyle.Bold), AutoSize=true });
            pnlAliciSag.Controls.Add(new Label { Text = "(Mesajı yanıtlamak için üzerine tıklayın)", Location = new Point(15, 50), Font = new Font("Segoe UI", 11, FontStyle.Italic), AutoSize=true, ForeColor = Color.DimGray });
            dgvAliciMesajlar = new DataGridView { Location = new Point(15, 80), Size = new Size(415, 600), SelectionMode = DataGridViewSelectionMode.FullRowSelect, AllowUserToAddRows=false, ReadOnly=true, AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill, AutoSizeRowsMode=DataGridViewAutoSizeRowsMode.AllCells, DefaultCellStyle = new DataGridViewCellStyle { WrapMode = DataGridViewTriState.True, Font=new Font("Segoe UI", 12) }, ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Font=new Font("Segoe UI", 12, FontStyle.Bold) }, RowTemplate = { MinimumHeight = 40 } }; 
            dgvAliciMesajlar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAliciMesajlar.CellClick += DgvAliciMesajlar_CellClick;
            pnlAliciSag.Controls.Add(dgvAliciMesajlar);
            pnlAlici.Controls.Add(pnlAliciSag);

            flpAlici = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(30) };
            pnlAlici.Controls.Add(flpAlici);
            this.Controls.Add(pnlAlici);

            pnlIlanDetay = new Panel { Dock = DockStyle.Fill, Visible = false, BackColor = Color.WhiteSmoke };
            Panel pnlDetayUst = new Panel { Dock = DockStyle.Top, Height = 90, BackColor = Color.SteelBlue };
            pnlDetayUst.Controls.Add(new Label { Text = "İlan Detayları", Location = new Point(30, 25), Font = new Font("Segoe UI", 22, FontStyle.Bold), AutoSize=true, ForeColor=Color.White });
            btnDetayGeri = new Button { Text = "İlan Vitrinine Dön", Location = new Point(pnlDetayUst.Width - 250, 25), Anchor = AnchorStyles.Right | AnchorStyles.Top, Width=220, Height=45, BackColor=Color.Gold, Font = new Font("Segoe UI", 12, FontStyle.Bold), FlatStyle=FlatStyle.Flat, Cursor = Cursors.Hand }; btnDetayGeri.Click += BtnDetayGeri_Click; pnlDetayUst.Controls.Add(btnDetayGeri);
            pnlIlanDetay.Controls.Add(pnlDetayUst);

            picDetay = new PictureBox { Location = new Point(60, 130), Size = new Size(550, 450), SizeMode = PictureBoxSizeMode.Zoom, BorderStyle=BorderStyle.FixedSingle, BackColor=Color.White }; pnlIlanDetay.Controls.Add(picDetay);
            
            lblDetayBaslik = new Label { Location = new Point(650, 130), Font = new Font("Segoe UI", 28, FontStyle.Bold), AutoSize=true, ForeColor=Color.MidnightBlue }; pnlIlanDetay.Controls.Add(lblDetayBaslik);
            lblDetayFiyat = new Label { Location = new Point(650, 190), Font = new Font("Segoe UI", 26, FontStyle.Bold), ForeColor=Color.SeaGreen, AutoSize=true }; pnlIlanDetay.Controls.Add(lblDetayFiyat);
            
            lblDetayBilgi = new Label { Location = new Point(650, 260), Font = new Font("Segoe UI", 16), AutoSize=true, Height=150 }; pnlIlanDetay.Controls.Add(lblDetayBilgi);

            pnlIlanDetay.Controls.Add(new Label { Text = "Satıcıya İletişim Mesajı:", Location = new Point(650, 430), Font=new Font("Segoe UI", 16, FontStyle.Bold), AutoSize=true });
            txtDetayMesaj = new TextBox { Location = new Point(650, 470), Width = 550, Height = 110, Multiline = true, Font = new Font("Segoe UI", 14) }; pnlIlanDetay.Controls.Add(txtDetayMesaj);
            btnDetayMesajGonder = new Button { Text = "Mesajı İlet", Location = new Point(1220, 470), Width = 150, Height = 110, BackColor = Color.Teal, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 14, FontStyle.Bold), Cursor = Cursors.Hand }; btnDetayMesajGonder.Click += BtnDetayMesajGonder_Click; pnlIlanDetay.Controls.Add(btnDetayMesajGonder);

            this.Controls.Add(pnlIlanDetay);
        }

        private void BtnGiris_Click(object sender, EventArgs e)
        {
            string kadi = txtKadi.Text.Trim();
            string sifre = txtSifre.Text.Trim();

            var k = db.Kullanicilar.Where(x => x.KullaniciAdi.ToLower() == kadi.ToLower() && x.Sifre == sifre).FirstOrDefault();
            if (k != null)
            {
                aktifKullaniciId = k.Id;
                aktifKullaniciRol = k.Rol;
                pnlGiris.Visible = false;
                
                if (k.Rol == "Satici") { 
                    pnlSatici.Visible = true; 
                    SaticiIlanlariniGetir(); 
                    MesajlariGuncelle(dgvSaticiMesajlar); 
                }
                else { 
                    pnlAlici.Visible = true; 
                    TumIlanlariGetir(""); 
                    MesajlariGuncelle(dgvAliciMesajlar); 
                }
                
                txtKadi.Text = ""; txtSifre.Text = "";
            }
            else { MessageBox.Show("Hatalı Giriş! Kullanıcı adı veya şifre yanlış."); }
        }

        private void BtnKayit_Click(object sender, EventArgs e)
        {
            string yeniKadi = txtYeniKadi.Text.Trim();
            string yeniSifre = txtYeniSifre.Text.Trim();

            if (string.IsNullOrEmpty(yeniKadi) || string.IsNullOrEmpty(yeniSifre)) { 
                MessageBox.Show("Kullanıcı adı ve şifre boş olamaz!"); 
                return; 
            }
            
            var varMi = db.Kullanicilar.Where(x => x.KullaniciAdi.ToLower() == yeniKadi.ToLower()).FirstOrDefault();
            if(varMi != null) {
                MessageBox.Show("Bu kullanıcı adı zaten alınmış!");
                return;
            }

            int yeniId = db.Kullanicilar.Count > 0 ? db.Kullanicilar.Max(x => x.Id) + 1 : 1;
            db.Kullanicilar.Add(new Kullanici { Id = yeniId, KullaniciAdi = yeniKadi, Sifre = yeniSifre, Rol = cmbRol.Text });
            db.SaveChanges(); 

            MessageBox.Show("Kayıt Başarılı! Sağ taraftan yeni bilgilerinizle hemen giriş yapabilirsiniz.");
            txtYeniKadi.Text = ""; txtYeniSifre.Text = "";
        }

        private void BtnCikis_Click(object sender, EventArgs e)
        {
            aktifKullaniciId = 0; aktifKullaniciRol = "";
            pnlAlici.Visible = false; pnlSatici.Visible = false; pnlIlanDetay.Visible = false;
            pnlGiris.Visible = true;
        }

        private void BtnResimSec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    secilenResimReferansi = Image.FromFile(ofd.FileName);
                    picSecilenResim.Image = secilenResimReferansi;
                }
            }
        }

        private void BtnIlanEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUrunBaslik.Text) || string.IsNullOrEmpty(txtUrunFiyat.Text)) return;
            int yeniId = db.Urunler.Count > 0 ? db.Urunler.Max(x => x.Id) + 1 : 1;
            
            int km = 0; int.TryParse(txtUrunKm.Text, out km);
            int model = 0; int.TryParse(txtUrunModel.Text, out model);

            Urun y = new Urun { 
                Id = yeniId, Baslik = txtUrunBaslik.Text, Fiyat = Convert.ToInt32(txtUrunFiyat.Text), 
                SaticiId = aktifKullaniciId, Tarih = DateTime.Now,
                Durum = cmbUrunDurum.Text, Km = km, SaseNo = txtUrunSaseNo.Text, ModelYili = model
            };
            if (secilenResimReferansi != null) y.UrunResmi = secilenResimReferansi;

            db.Urunler.Add(y);
            db.SaveChanges(); 
            
            txtUrunBaslik.Text = ""; txtUrunFiyat.Text = ""; txtUrunKm.Text = ""; txtUrunSaseNo.Text = ""; txtUrunModel.Text = "";
            picSecilenResim.Image = null; secilenResimReferansi = null;
            
            SaticiIlanlariniGetir(); 
        }

        private Panel KutuOlustur(Urun urun, bool isSatici)
        {
            Panel card = new Panel { Width = 280, Height = 360, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Margin = new Padding(20) };
            
            PictureBox pic = new PictureBox { Width = 280, Height = 190, Location = new Point(0,0), SizeMode = PictureBoxSizeMode.Zoom, BackColor=Color.White };
            if (urun.UrunResmi != null) pic.Image = urun.UrunResmi;
            card.Controls.Add(pic);

            Label lblBaslik = new Label { Text = urun.Baslik, Location = new Point(10, 200), Font = new Font("Segoe UI", 14, FontStyle.Bold), AutoSize=false, Width=260, Height=30 };
            card.Controls.Add(lblBaslik);

            Label lblFiyat = new Label { Text = urun.Fiyat.ToString("N0") + " TL", Location = new Point(10, 235), Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor=Color.SeaGreen, AutoSize=true };
            card.Controls.Add(lblFiyat);

            if (isSatici)
            {
                Button btnSil = new Button { Text = "✖ İlanı Kaldır", Location = new Point(15, 290), Width=250, Height=50, BackColor=Color.Crimson, ForeColor=Color.White, Font=new Font("Segoe UI", 12, FontStyle.Bold), FlatStyle=FlatStyle.Flat, Cursor=Cursors.Hand };
                btnSil.Click += (s, e) => { db.Urunler.Remove(urun); db.SaveChanges(); SaticiIlanlariniGetir(); };
                card.Controls.Add(btnSil);
            }
            else
            {
                Button btnIncele = new Button { Text = "🔍 Detayları İncele", Location = new Point(15, 290), Width=250, Height=50, BackColor=Color.SteelBlue, ForeColor=Color.White, Font=new Font("Segoe UI", 12, FontStyle.Bold), FlatStyle=FlatStyle.Flat, Cursor=Cursors.Hand };
                btnIncele.Click += (s, e) => { IlanDetayAc(urun); };
                card.Controls.Add(btnIncele);
            }

            return card;
        }

        private void SaticiIlanlariniGetir()
        {
            flpSatici.Controls.Clear();
            var ilanlar = db.Urunler.Where(x => x.SaticiId == aktifKullaniciId).ToList();
            foreach (var item in ilanlar) { flpSatici.Controls.Add(KutuOlustur(item, true)); }
        }

        private void TumIlanlariGetir(string aramaMetni)
        {
            flpAlici.Controls.Clear();
            var ilanlar = db.Urunler.AsQueryable();
            if (!string.IsNullOrEmpty(aramaMetni))
            {
                ilanlar = ilanlar.Where(x => x.Baslik.ToLower().Contains(aramaMetni.ToLower()));
            }
            foreach (var item in ilanlar.ToList()) { flpAlici.Controls.Add(KutuOlustur(item, false)); }
        }

        private void TxtAra_TextChanged(object sender, EventArgs e)
        {
            TumIlanlariGetir(txtAra.Text);
        }

        private void IlanDetayAc(Urun u)
        {
            aktifIncelenenUrun = u;
            pnlAlici.Visible = false;
            pnlIlanDetay.Visible = true;

            picDetay.Image = u.UrunResmi;
            lblDetayBaslik.Text = u.Baslik;
            lblDetayFiyat.Text = u.Fiyat.ToString("N0") + " TL";
            
            lblDetayBilgi.Text = $"Ürün Durumu: {u.Durum}\n\n" +
                                 $"Model (Yıl): {u.ModelYili}\n\n" +
                                 $"Kilometre: {(u.Km == 0 ? "Yok" : u.Km.ToString("N0") + " KM")}\n\n" +
                                 $"Şase No / Kodu: {u.SaseNo}\n\n" +
                                 $"İlan Tarihi: {u.Tarih.ToShortDateString()}";
        }

        private void BtnDetayGeri_Click(object sender, EventArgs e)
        {
            pnlIlanDetay.Visible = false;
            pnlAlici.Visible = true;
            aktifIncelenenUrun = null;
            txtDetayMesaj.Text = "";
        }

        private void BtnDetayMesajGonder_Click(object sender, EventArgs e)
        {
            if (aktifIncelenenUrun != null && !string.IsNullOrEmpty(txtDetayMesaj.Text))
            {
                int yeniId = db.Mesajlar.Count > 0 ? db.Mesajlar.Max(x => x.Id) + 1 : 1;
                db.Mesajlar.Add(new Mesaj { Id = yeniId, GonderenId = aktifKullaniciId, AliciId = aktifIncelenenUrun.SaticiId, Icerik = txtDetayMesaj.Text, Tarih = DateTime.Now });
                db.SaveChanges();
                MessageBox.Show("Mesajınız Satıcıya Başarıyla İletildi!");
                txtDetayMesaj.Text = "";
            }
        }

        private void MesajlariGuncelle(DataGridView dgv)
        {
            dgv.DataSource = null;
            var mesajListesi = db.Mesajlar.Where(x => x.AliciId == aktifKullaniciId).Select(m => new {
                m.Id,
                Gönderen = db.Kullanicilar.Where(k=>k.Id == m.GonderenId).FirstOrDefault().KullaniciAdi,
                Tarih = m.Tarih.ToString("dd.MM.yyyy HH:mm"),
                Mesaj = m.Icerik,
                GonderenId = m.GonderenId 
            }).OrderByDescending(x => x.Tarih).ToList();

            dgv.DataSource = mesajListesi;
            
            if (dgv.Columns["Id"] != null) dgv.Columns["Id"].Visible = false;
            if (dgv.Columns["GonderenId"] != null) dgv.Columns["GonderenId"].Visible = false;

            if (dgv.Columns["Gönderen"] != null) dgv.Columns["Gönderen"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            if (dgv.Columns["Tarih"] != null) dgv.Columns["Tarih"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            if (dgv.Columns["Mesaj"] != null) dgv.Columns["Mesaj"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void DgvSaticiMesajlar_CellClick(object sender, DataGridViewCellEventArgs e) { MesajaYanitVer(dgvSaticiMesajlar, e.RowIndex); }
        private void DgvAliciMesajlar_CellClick(object sender, DataGridViewCellEventArgs e) { MesajaYanitVer(dgvAliciMesajlar, e.RowIndex); }

        private void MesajaYanitVer(DataGridView dgv, int rowIndex)
        {
            if (rowIndex >= 0)
            {
                int gonderenId = (int)dgv.Rows[rowIndex].Cells["GonderenId"].Value;
                string mesaj = dgv.Rows[rowIndex].Cells["Mesaj"].Value.ToString();
                string gonderenAd = dgv.Rows[rowIndex].Cells["Gönderen"].Value.ToString();

                string cevap = Interaction.InputBox($"'{gonderenAd}' adlı kullanıcının mesajı:\n{mesaj}\n\nYanıtınızı yazın:", "Mesaja Hızlı Yanıt", "");

                if (!string.IsNullOrEmpty(cevap))
                {
                    int yeniId = db.Mesajlar.Count > 0 ? db.Mesajlar.Max(x => x.Id) + 1 : 1;
                    db.Mesajlar.Add(new Mesaj { Id = yeniId, GonderenId = aktifKullaniciId, AliciId = gonderenId, Icerik = cevap, Tarih = DateTime.Now });
                    db.SaveChanges();
                    MessageBox.Show("Yanıtınız gönderildi!");
                }
            }
        }
    }

    public class OtoPazarEntities
    {
        public MockDbSet<Kullanici> Kullanicilar { get; set; } = new MockDbSet<Kullanici>();
        public MockDbSet<Urun> Urunler { get; set; } = new MockDbSet<Urun>();
        public MockDbSet<Mesaj> Mesajlar { get; set; } = new MockDbSet<Mesaj>();
        public void SaveChanges() { } 
    }

    public class MockDbSet<T> : List<T>
    {
        public void RemoveRange(IEnumerable<T> items)
        {
            foreach (var item in items.ToList()) { this.Remove(item); }
        }
    }

    public class Kullanici
    {
        public int Id { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public string Rol { get; set; } 
    }

    public class Urun
    {
        public int Id { get; set; }
        public string Baslik { get; set; }
        public int Fiyat { get; set; }
        public int SaticiId { get; set; }
        public DateTime Tarih { get; set; }
        public Image UrunResmi { get; set; }
        public string Durum { get; set; } 
        public int Km { get; set; }
        public string SaseNo { get; set; }
        public int ModelYili { get; set; } 
    }

    public class Mesaj
    {
        public int Id { get; set; }
        public int GonderenId { get; set; }
        public int AliciId { get; set; }
        public string Icerik { get; set; }
        public DateTime Tarih { get; set; }
    }
}
