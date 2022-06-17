using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class Data
    {

        public double[] input { get; set; }
        //veri girişlerimiz bu proje için bir tane
        //ancak birden fazla girişimiz olabilir.
        //bunun için dizi tanımlaması yapıldı.

        public int output { get; set; }
        //giriş sayımız değişken olabilir, ancak mutlak bir çıktı vardır.
        //bu sınıflandırmada sonuç olarak değerlendirilebilir.
        //verilen girdiler için beklenen çıktıyı temsil eder.

        public Data(double[] input, int output)
        {
            this.input = input;
            this.output = output;
        }
    }

    class Neuron
    {
        public double bias { get; set; }
        //her nöronun bir bias değeri mevcuttur.

        public double[] w { get; set; }
        //bu proje için tek bir nöron olduğu için ağırlıklar nöron ağırlığı olarak tutulabilir.

        public Function function { get; set; }
        //her nöronun bir toplama ve aktivasyon fonksiyonu vardır.
        //bu proje için toplama fonksiyonu tüm nöronlar için ortaktır.
        //aktivasyon fonksiyonu ise değişkenlik gösterebilir şekilde yapılmıştır.
        //aktivasyon fonksiyonu dependency inversion ile nöron tanımlanırken verilmektedir.

        public Neuron(int dimensation, double bias, Function function)
        {
            w = new double[dimensation];
            //w değeri, nöronumuzun ağırlıklarını tutan bir dizi.
            //dimensation burada kaçtane ağırlığımızın olduğunu bize gösteriyor.
            //ağımız tek nörondan oluştuğu için dimensation aynı zamanda veri giriş sayısına da verir.

            for (int i = 0; i < dimensation; i++)
            {
                w[i] = new Random().NextDouble();
                //alınan veri sayısı kadar w ağırlıkları random olarak atanır.
                //burda dimensation input katmanındaki veri sayısıdır.
                //bias değeri hesaba katılmamıştır. bias için ayrı bir işlem mi gerçekleştiriliyor acaba??
            }
            this.bias = bias;
            this.function = function;
        }

        public String getW()
        {
            //getW fonksiyonumuz, ağırlık değerlerini string bir şekilde yanyana yazdırır.
            //geri döndürür.

            string data = "";
            for (int i = 0; i < w.Length; i++)
            {
                data = data + " " + w[i];
            }
            return data;
        }


        public double getClass(int classNumber, int classIndex)
        {
            if (classNumber == classIndex)
                return 1;
            else
                return -1;
        }
    }

    abstract class Function
    {
        public double net(double[] input, double[] w, double bias)//toplamfonksiyonu
        {
            double sum = 0;
            for (int i = 0; i < input.Length; i++)
            {
                sum += input[i] * w[i];
            }
            return sum + (w[w.Length - 1] * bias); //biası en son olarak ekledik.
        }

        public abstract double calculate(double net);
    }
    class Binary : Function
    {
        public override double calculate(double net)
        {
            if (net > 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }

    class Sigmoid : Function
    {
        public override double calculate(double net)
        {



            return 1 / (1 + Math.Pow(Math.E, -net));
            //sigmoid fonksiyonuna denk gelir.
        }
        public double derivateCalculate(double sdsdsd)
        {
            return 0;
        }
    }


    public partial class Form1 : Form
    {
        List<Data> dataList;
        int classCount;

        public Form1()
        {

            InitializeComponent();

            dataList = new List<Data>();
            double[] d1 = { 1, 2 };
            double[] d2 = { 2, 3 };
            double[] d3 = { -1, 0 };
            double[] d4 = { -1, -2 };
            double[] d5 = { 1, 0 };
            double[] d6 = { -1, 1 };

            //bunlar giriş verileri olmakta
            //2 tane giriş kısmı var.

            dataList.Add(new Data(d1, 1));
            dataList.Add(new Data(d2, 1));
            dataList.Add(new Data(d3, -1));
            dataList.Add(new Data(d4, -1));
            dataList.Add(new Data(d5, 1));
            dataList.Add(new Data(d6, -1));

            //dataList giriş ve çıkış verilerimizi içerir.
            //giriş verisi dizi, 2 girişi olabilir nöronun tıpkı burda olduğu gibi. 

            HashSet<int> classArray = new HashSet<int>(); //girdi sayım
            for (int i = 0; i < dataList.Count; i++)
            {
                classArray.Add(dataList[i].output);
            }
            classCount = classArray.Count;
        }

        private void button1_Click(object sender, EventArgs e) //butona tıklandığında bu fonksiyon çalışıyor.
        {
            sinleLayerSingleNeuron(0.1, dataList[0].input.Length + 1, 1);
            //gönderilen parametrelerde öğrenme katsayısı, giriş dizisinin boyutu +1, bias değeri
            //giriş değerinin +1 ifadesi nörona giriş sayısını veriyor.
            //nörona giriş sayısı != veriseti örnek sayısı
            //nörona giriş sayısı = veriseti örneklerinden her birinde kaç tane giriş verisi bulunduğu.

        }
        private void button2_Click(object sender, EventArgs e)
        {
            singleLayerMultiNeuron(0.1, dataList[0].input.Length + 1, 1);
        }

        public void sinleLayerSingleNeuron(double c, int dimensation, double bias)
        {
            //dimensation ağırlık sayısına denk geliyor.
            //c katsayısı anlamına geliyor.
            //bias değeride otomatik 1 olarak giriliyor.

            Neuron neuron = new Neuron(dimensation, bias, new Binary());
            //Tek katmanlı ağ olduğu için, tek bir nöronumuz var.
            //nöronumuzun veri giriş sayısı, bias değeri ve aktivasyon fonksiyonu mevcut.
            //toplama fonksiyonu tüm nöronlar için ortak.


            while (true)
            //epoch sayımıza denk gelicek aslında. Sonlandırma kriteri olabilir.
            {
                double error = 0;
                //error değişkenimiz bizim toplam hata değerimizi ölçecek.

                for (int i = 0; i < dataList.Count; i++) //veri sayısı
                //datalist'im en yukarıda tanımlı ve erişime açık. İçinde girdi verilerim bulunuyor.
                {
                    double net = neuron.function.net(dataList[i].input, neuron.w, neuron.bias);
                    //nöronumuzun toplama fonksiyonundan gelen sonucunu tutuyor.
                    //toplama fonksiyonu girişlerimiz, ağırlıklarımız ve bias değeri ile bağlantılıdır.

                    double fnet = neuron.function.calculate(net);
                    //fnet değişkeni nöronun aktivasyon fonksiyonuna gönderilen toplama sonucunu hesaplıyor.


                    for (int j = 0; j < (dimensation - 1); j++)
                    //ağırlık boyutu, girdi boyutundan 1 fazladır. oda biasın hakkı.
                    {
                        neuron.w[j] = neuron.w[j] + c * (dataList[i].output - fnet) * dataList[i].input[j];
                        //ağırlık = ağırlık + c*(hata)*xi
                        //guncelAğırlık = eskiAğırlık + öğrenmeKatsayısı*(istenenSonuç - çıkanSonuç)*girdi
                        //burda istenenSonuç, dataList tanımlamasında 1 ve -1 olarak verilmiştir. Yukarıda var.
                        //dataList[i].input[j] bu şekilde yazılmasının sebebi,
                        //datalist bir liste, ve içerisinde dizileri tutmaktadır.
                        //her dizi girişi ve istenenSonucu tutmaktadır.

                    }
                    neuron.w[dimensation - 1] = neuron.w[dimensation - 1] + c * (dataList[i].output - fnet) * bias;
                    //diyelimki nöronumuzun inputu 4, bias girişi ile 5 oluyor.
                    //girişler sıfırdan itibaren verildiği için en sonuncu değer boşta kalıyor.
                    //onu da manuel olarak biasa atıyoruz.
                    //yani ağırlıklar dizinin 4. indisine denk geliyor. Çünkü sıfırdan başlıyor diziler...
                    //bu kısımdada nöronun bias ağırlığını düzenleyelim.
                    //güncelAğırlık = eskiAğırlık + öğrenmeKatsayısı * (hata)*bias
                    //yukarı satırdaki kod ile bias ağırlığını güncelliyoruz.

                    error = error + Math.Pow(dataList[i].output - fnet, 2) / 2;
                    Console.WriteLine("------------------------------> " + error);
                    //Console.WriteLine("Döngüde devam ediyor.." + error);

                    //hata fonksiyonumuzda bu şekilde.
                    //hata += hatanın karesini al ve ikiye böl.
                    //bu hata değeri bizim durdurma kriterimiz, yani başarı kriterimiz oluyor.
                }
                if (error < 0.1)
                {
                    break;
                    //eğer hatamız yeterince az ise, eğitimi durduruyoruz.
                }
            }
            MessageBox.Show(neuron.getW() + "");
            //burda da nöronun ağırlıkları ekrana bastırılıyor.
        }

        public void singleLayerMultiNeuron(double c, int dimensation, double bias)
        {
            List<Neuron> neuronList = new List<Neuron>();
            for (int i = 0; i < classCount; i++) //veri sayısı kadar dönecek. çıktı katmanı
            {
                neuronList.Add(new Neuron(dimensation, bias, new Binary()));
            }

            while (true)
            {
                double error = 0;
                for (int i = 0; i < dataList.Count; i++) //örnek sayısı
                {
                    for (int j = 0; j < classCount; j++) //nöronlar için tek tek dönücek.
                    {
                        Neuron neuron = neuronList[j];

                        double net = neuron.function.net(dataList[i].input, neuron.w, neuron.bias);

                        double fnet = neuron.function.calculate(net);

                        for (int k = 0; k < dimensation - 1; k++)
                        {
                            neuron.w[k] = neuron.w[k] + c * (neuron.getClass(dataList[i].output, j) - fnet) * dataList[i].input[k];

                        }

                        neuron.w[dimensation - 1] = neuron.w[dimensation - 1] + c * (neuron.getClass(dataList[i].output, j) - fnet) * neuron.bias;

                        error = error + Math.Pow(neuron.getClass(dataList[i].output, j) - fnet, 2) / 2;

                    }
                }



                if (error < 0.1 * classCount)
                {
                    break;
                    //eğer hatamız yeterince az ise, eğitimi durduruyoruz.
                }
            }
        }

        int maxIteration = 1000;
        public void multiLayerMultiNeuron(double c, int dimensation, double bias, int neuronSize)
        {
            List<Neuron> inputNeuronList = new List<Neuron>();
            List<Neuron> hidenNeuronList = new List<Neuron>();
            double[] fnetV = new double[neuronSize];    //girdi katmanın çıktıları ara katmana girdi olarak kullanılacak


            for (int i = 0; i < neuronSize; i++)    //girdi katmanına nöronlar ekleniyor. 
            {
                inputNeuronList.Add(new Neuron(dimensation, bias, new Sigmoid()));
            }
            for (int i = 0; i < classCount; i++)    //ara katmanda nöronlar ekleniyor.
            {
                hidenNeuronList.Add(new Neuron(dimensation, bias, new Sigmoid()));
            }
            int iteration = 0;
            while (true)
            {
                iteration++;
                for (int i = 0; i < classCount; i++) //input
                {
                    for (int j = 0; j < classCount; j++) //gizli katman nöron
                    {
                        Neuron hidenNeuron = hidenNeuronList[j];
                        for (int k = 0; k < neuronSize; k++) //giriş katman nöronları
                        {
                            Neuron neuron = inputNeuronList[k];
                            double netV = neuron.function.net(dataList[i].input, neuron.w, neuron.bias);
                            fnetV[k] = neuron.function.calculate(netV);
                        }

                        double netW = hidenNeuron.function.net(fnetV, hidenNeuron.w, hidenNeuron.bias);
                        double fnetW = hidenNeuron.function.calculate(netW);

                        for (int k = 0; k < neuronSize; k++)
                        {
                            hidenNeuron.w[k] = hidenNeuron.w[k] + c * (hidenNeuron.getClass(dataList[i].output, j) - fnetW) * fnetV[k];
                        }
                        hidenNeuron.w[dimensation - 1] = hidenNeuron.w[neuronSize] + c * (hidenNeuron.getClass(dataList[i].output, j) - fnetW) * bias;
                        Sigmoid cont = (Sigmoid)hidenNeuron.function;
                        //error = error + Math.Pow(hidenNeuron.getClass(dataList[i].output, j) - fnetW, 2) / 2;

                        for (int k = 0; k < neuronSize; k++)
                        {
                            Neuron inputNeuron = inputNeuronList[k];
                            for (int m = 0; m < dimensation - 1; m++)
                            {
                                inputNeuron.w[m] = inputNeuron.w[m] + c * (hidenNeuron.getClass(dataList[i].output, j) - fnetW) * fnetV[k] * dataList[i].input[m];
                                inputNeuron.w[dimensation] = inputNeuron.w[dimensation] + c * (hidenNeuron.getClass(dataList[i].output, j) - fnetW) * cont.derivateCalculate(fnetV[k]) * inputNeuron.bias;

                            }
                        }
                    }
                }
            }
        }
    }


/*    
      knas
     ax + by + c = 0    a b ve c ->w1 , w2 , w0 - bias  x y z -> girisler

   deltaW = c*(d-o)*x perceptron öğrenme kuralı
    wi = wi-1 + deltaW
    

    Genel programın çalışmasını inceleyelim.
    DataList oluşturuluyor.
        DataList bir liste.
        Listenin her bir elemanı; input dizisi, ve karşılığında beklenen sonucundan oluşuyor.

    Yapımız tek katmanlı olduğu için bir nöron oluşturuluyor.
        nöronun giriş değeleri olarak bias değeri, aktivasyon fonksiyonu ve kaç tane giriş kanalı olduğu söyleniyor.
        bu giriş kanalları ağırlıklarak denk gelecek.

    Nöron oluşturulduktan sonra verisetindeki örnekler işleme tabi tutuluyor.
        Her verisetindeki örnek fonksiyona sokuluyor.
        Toplam fonksiyonu ile bir net değeri oluşuyor.
        Net değeri aktivasyon fonksiyonuna sokularak Fnet çıktısı üretiliyor.
        Daha sonra guncelAğırlık = eskiAğırlık + öğrenmeKatsayısı * (beklenen - fnet) * ağırlıktanGirenVeri işlemi
            uygulanarak ağırlık değişimleri gerçekleştiriliyor.
        Her verisetindeki örnek için ((beklenen sonuç - fnet sonucu)^2)/2 işlemi uygulanarak toplam hataya ekleniyor.
        Eğer toplam hata belirlenen kriterin altına düşmüş ise eğitim sonlandırılıyor.
            Eğer düşmemiş ise eğitim devam ediyor. kod üzerinde while döngüsü bu kısımla ilgileniyor.
            her bir while turu, 1 epoch'a denk gelmektedir.
     
     2 sınıf olduğunda 1 nöron sınıflandırma için yeterli
     * Ama sınıf sayısı 3 olduğunda 3 tane nöron kullanmak zorundayız.
     * 2 den sonra hepsi, kaç çıkış sınıfı varsa o kadar nöron olmalıdır.
     * 
     * 
     * her nöronun kendi net değeri vardır.
     * 
     * 
     * 
     * 
     * 
     * 
     *
    sürekli değer fonksiyonları normalize edildikten sonra kullanılmalıdır.
     sürekli değerde bölgelerin merkezlerine eşit uzaklıkta, olabildiğince eşit bir şekilde ayırmaya çalışıyor.
     binary'de ise direk böler bölmez duruyor.
     ayrık değerde iki sınıfı ayırdığı anda hata 0 oluyor,
     sürekli de ise bölgelerin merkezlerine odaklanmaya çalışıyor.
     0'a ulaşamıyor. hep yakınsıyor.
     tek bir doğru üzerine x y x olarak denk geldiğinde x ve y'ler ayrılamaz.
    lineer ayrıştırma hatası, 
     2 sınıf ve birden çok katman olduğunda 2 sınıf oluşturulur.
     birden çok katman kullanıldığında başka bir uzaya taşımış oluyoruz.
     lineer olarak ayrıştıramadığımz zaman çok katmanlı yapıya geçmek lazım.
     başka bir uzaya taşıdığımızda artık doğrular yok eğriler var.
     nöron sayısı artarsa, eğrinin kıvrımlaşması artar.
     gizli katmanda 4 sınıf için 4 nöron koyulmalı dedi galiba..
     gizli katmanda nöron sayısı artarsa sınıflandırma başarısı da artabilir.
     çünkü eğrilerin kıvrımları da artıyor.
     böylelikle ortada olan bir kümeyi ayırabiliyoruz.
     uzay dönüşümü ile bölgeler çok daha kolay ayrılabilir bir şekle dönüşebilir.
     çok katmanlı ağlarda ilk katman yine aynı şekilde hesaplama yapılır.
     ara katmandaki ağırlıklar ayrı bir şekilde kullanılır.
     ilk katmanın çıktıları, gizli katmana girdi olarak eklenecektir.
     gizli katman nöron sayısı, giriş katmandan bağımsızdır.
    
     Toplama fonksiyonunda bias değeri de hesaba katılmalı, onu unutma!!
     Toplama fonksiyonunun kullanım amacı: elimizdeki tüm girdileri tek bir çıktıda toplamak isteriz.
     Aktivasyon fonksiyonu: istenilen aralığa düşürülüyor. Doğru mu yanlış mı gibi tahminleri yaptırmak istiyor.
     Öğrenme kuralı: Deltaw = c*(d-0)xi --> öğrenme katsayısı, beklenen değer, çıktı, giriş. Delta öğrenme kuralı kullanılmıştır.
         öğrenme katsayısı c'nin yüksek veya düşük olması istenmez. 
         en optimum değeri alınıyor.
     Hata fonksiyonu: Toplam hata bulunur.
         hata değerinde karesinin alınmasının sebebi pozitif sınırlara düşürmek.
         birinin -1, diğerinin hatası 1 olduğunda hata 0, yani yok gibi görünür.
         (d-o)^2

    
*/
}
