using System;
using System.Globalization;
using System.Text;

enum ViTriHaiDuongTron { TrungNhau, ChuaNhau, TiepXucTrong, CatNhau, TiepXucNgoai, NgoaiNhau }

enum ViTriDiemDuongTron { BenTrong, TrenDuongTron, BenNgoai }

class Diem
{
    public double X { get; set; }
    public double Y { get; set; }

    public Diem() { X = 0; Y = 0; }
    public Diem(double x, double y) { X = x; Y = y; }

    public double KhoangCach(Diem p)
    {
        double dx = X - p.X;
        double dy = Y - p.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    public void Nhap()
    {
        X = DuongTron.NhapSo("Tọa độ x: ");
        Y = DuongTron.NhapSo("Tọa độ y: ");
    }

    public override string ToString()
    {
        return "(" + X.ToString("0.##", CultureInfo.InvariantCulture) + "; "
                   + Y.ToString("0.##", CultureInfo.InvariantCulture) + ")";
    }
}

class DuongTron
{
    private const double EPS = 1e-9;   
    private Diem tam;
    private double banKinh;

    public DuongTron()                       
    {
        tam = new Diem(0, 0);
        banKinh = 1;
    }

    public DuongTron(Diem tam, double banKinh) 
    {
        Tam = tam;
        BanKinh = banKinh;
    }

    public DuongTron(double x, double y, double banKinh)
        : this(new Diem(x, y), banKinh)
    { }

    public Diem Tam
    {
        get { return tam; }
        set
        {
            if (value == null)
                throw new ArgumentNullException("Tam", "Tâm không được null.");
            tam = new Diem(value.X, value.Y);  
        }
    }

    public double BanKinh
    {
        get { return banKinh; }
        set
        {
            if (value <= 0)
                throw new ArgumentException("Bán kính phải lớn hơn 0.");
            banKinh = value;
        }
    }

    public void Nhap()
    {
        double x = NhapSo("Tọa độ x của tâm: ");
        double y = NhapSo("Tọa độ y của tâm: ");
        double r;
        while (true)
        {
            r = NhapSo("Bán kính (> 0): ");
            if (r > 0) break;
            Console.WriteLine("Bán kính phải lớn hơn 0, nhập lại.");
        }
        tam = new Diem(x, y);
        banKinh = r;
    }

    public void Xuat()
    {
        Console.WriteLine(this);
    }

    public override string ToString()
    {
        return "Đường tròn tâm I" + tam + ", bán kính R = "
             + banKinh.ToString("0.##", CultureInfo.InvariantCulture);
    }

    public static double NhapSo(string loiNhac)
    {
        while (true)
        {
            Console.Write(loiNhac);
            string s = Console.ReadLine();
            if (s != null) s = s.Trim().Replace(',', '.');

            double so;
            if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out so))
                return so;
            Console.WriteLine("Phải nhập một số, nhập lại.");
        }
    }

    public double DienTich()
    {
        return Math.PI * banKinh * banKinh;
    }

    public double ChuVi()
    {
        return 2 * Math.PI * banKinh;
    }

    public ViTriHaiDuongTron ViTriTuongDoi(DuongTron kia)
    {
        double d = tam.KhoangCach(kia.tam);              // khoảng cách hai tâm
        double tong = banKinh + kia.banKinh;
        double hieu = Math.Abs(banKinh - kia.banKinh);

        if (d < EPS && hieu < EPS) return ViTriHaiDuongTron.TrungNhau;
        if (d > tong + EPS) return ViTriHaiDuongTron.NgoaiNhau;
        if (Math.Abs(d - tong) <= EPS) return ViTriHaiDuongTron.TiepXucNgoai;
        if (d < hieu - EPS) return ViTriHaiDuongTron.ChuaNhau;
        if (Math.Abs(d - hieu) <= EPS) return ViTriHaiDuongTron.TiepXucTrong;
        return ViTriHaiDuongTron.CatNhau;
    }

    public ViTriDiemDuongTron ViTriTuongDoi(Diem p)
    {
        double d = tam.KhoangCach(p);
        if (Math.Abs(d - banKinh) <= EPS) return ViTriDiemDuongTron.TrenDuongTron;
        if (d < banKinh) return ViTriDiemDuongTron.BenTrong;
        return ViTriDiemDuongTron.BenNgoai;
    }
}

class Program
{
    static string MoTa(ViTriHaiDuongTron v)
    {
        switch (v)
        {
            case ViTriHaiDuongTron.TrungNhau: return "Hai đường tròn trùng nhau";
            case ViTriHaiDuongTron.ChuaNhau: return "Một đường tròn chứa đường tròn kia (không có điểm chung)";
            case ViTriHaiDuongTron.TiepXucTrong: return "Hai đường tròn tiếp xúc trong";
            case ViTriHaiDuongTron.CatNhau: return "Hai đường tròn cắt nhau tại 2 điểm";
            case ViTriHaiDuongTron.TiepXucNgoai: return "Hai đường tròn tiếp xúc ngoài";
            default: return "Hai đường tròn ở ngoài nhau";
        }
    }

    static string MoTa(ViTriDiemDuongTron v)
    {
        switch (v)
        {
            case ViTriDiemDuongTron.BenTrong: return "Điểm nằm bên trong đường tròn";
            case ViTriDiemDuongTron.TrenDuongTron: return "Điểm nằm trên đường tròn";
            default: return "Điểm nằm bên ngoài đường tròn";
        }
    }

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("=== 1. Khởi tạo ===");
        DuongTron c0 = new DuongTron();
        DuongTron c1 = new DuongTron(new Diem(2, 3), 4);
        DuongTron c2 = new DuongTron(-1, 0, 2.5);
        c0.Xuat();
        c1.Xuat();
        c2.Xuat();

        Console.WriteLine();
        Console.WriteLine("=== 2. Nhập đường tròn thứ nhất ===");
        DuongTron a = new DuongTron();
        a.Nhap();
        Console.WriteLine("=== Nhập đường tròn thứ hai ===");
        DuongTron b = new DuongTron();
        b.Nhap();

        Console.WriteLine();
        Console.WriteLine("=== 3. Thông tin ===");
        foreach (DuongTron c in new DuongTron[] { a, b })
        {
            c.Xuat();
            Console.WriteLine("   Diện tích = " + c.DienTich().ToString("F2", CultureInfo.InvariantCulture));
            Console.WriteLine("   Chu vi    = " + c.ChuVi().ToString("F2", CultureInfo.InvariantCulture));
        }

       
        Console.WriteLine();
        Console.WriteLine("=== 4. Vị trí tương đối của hai đường tròn ===");
        Console.WriteLine(MoTa(a.ViTriTuongDoi(b)));

        
        Console.WriteLine();
        Console.WriteLine("=== 5. Nhập một điểm, so với đường tròn thứ nhất ===");
        Diem p = new Diem();
        p.Nhap();
        Console.WriteLine("Điểm " + p + ": " + MoTa(a.ViTriTuongDoi(p)));

        Console.ReadKey();
    }
}