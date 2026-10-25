using System;
using System.Globalization;
using System.Text;

class DaThuc
{
    private double[] heSo;     
    public DaThuc(int bac)
    {
        if (bac < 0)
            throw new ArgumentException("Bậc của đa thức phải >= 0.");
        heSo = new double[bac + 1];      
    }

    public int Bac
    {
        get { return heSo.Length - 1; }
    }

    public double this[int k]
    {
        get
        {
            KiemTraChiSo(k);
            return heSo[k];
        }
        set
        {
            KiemTraChiSo(k);
            heSo[k] = value;
        }
    }

    private void KiemTraChiSo(int k)
    {
        if (k < 0 || k > Bac)
            throw new IndexOutOfRangeException("Chỉ số k phải từ 0 đến " + Bac + ".");
    }

    public static DaThuc operator +(DaThuc a, DaThuc b)
    {
        int bacMax = Math.Max(a.Bac, b.Bac);
        DaThuc kq = new DaThuc(bacMax);
        for (int k = 0; k <= bacMax; k++)
        {
            double ak = (k <= a.Bac) ? a[k] : 0;    
            double bk = (k <= b.Bac) ? b[k] : 0;
            kq[k] = ak + bk;
        }
        return kq;
    }

    public static DaThuc operator -(DaThuc a, DaThuc b)
    {
        int bacMax = Math.Max(a.Bac, b.Bac);
        DaThuc kq = new DaThuc(bacMax);
        for (int k = 0; k <= bacMax; k++)
        {
            double ak = (k <= a.Bac) ? a[k] : 0;
            double bk = (k <= b.Bac) ? b[k] : 0;
            kq[k] = ak - bk;
        }
        return kq;
    }


    public double TinhGiaTri(double b)
    {
        double kq = 0;
        for (int k = Bac; k >= 0; k--)
            kq = kq * b + heSo[k];
        return kq;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int k = Bac; k >= 0; k--)
        {
            double h = heSo[k];
            if (h == 0)
                continue;

            if (sb.Length > 0)
                sb.Append(h > 0 ? " + " : " - ");
            else if (h < 0)
                sb.Append("-");

            double giaTriTuyetDoi = Math.Abs(h);
            if (k == 0)
            {
                sb.Append(giaTriTuyetDoi.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                if (giaTriTuyetDoi != 1)
                    sb.Append(giaTriTuyetDoi.ToString(CultureInfo.InvariantCulture));
                sb.Append("x");
                if (k > 1)
                    sb.Append("^" + k);
            }
        }
        return sb.Length == 0 ? "0" : sb.ToString();
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

   
        DaThuc p = new DaThuc(2);
        p[0] = 1;
        p[1] = 2;
        p[2] = 3;

    
        DaThuc q = new DaThuc(3);
        q[0] = 4;
        q[1] = -1;
        q[3] = 5;

        Console.WriteLine("P(x) = " + p + "   (bậc " + p.Bac + ")");
        Console.WriteLine("Q(x) = " + q + "   (bậc " + q.Bac + ")");

        DaThuc tong = p + q;
        DaThuc hieu = p - q;
        Console.WriteLine("P + Q = " + tong + "   (bậc " + tong.Bac + ")");
        Console.WriteLine("P - Q = " + hieu + "   (bậc " + hieu.Bac + ")");

        Console.WriteLine();
        Console.WriteLine("P(2)     = " + p.TinhGiaTri(2));
        Console.WriteLine("Q(2)     = " + q.TinhGiaTri(2));
        Console.WriteLine("(P+Q)(2) = " + tong.TinhGiaTri(2));
        Console.WriteLine("(P-Q)(2) = " + hieu.TinhGiaTri(2));

        Console.WriteLine();
        Console.WriteLine("Hệ số a1 của P = " + p[1]);      

      
        try
        {
            Console.WriteLine(p[5]);
        }
        catch (IndexOutOfRangeException ex)
        {
            Console.WriteLine("Lỗi: " + ex.Message);
        }

        Console.ReadKey();
    }
}