using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace base64
{
    class Program
    {
        static Byte[] GetBytesFromBinaryString(String input_binary)
        {
            var list = new List<Byte>();

            for (int i = 0; i < input_binary.Length; i += 8)
            {
                String t = input_binary.Substring(i, 8);

                list.Add(Convert.ToByte(t, 2));
            }

            return list.ToArray();
        }


        static String Base64Encoder(String data_in)
        {
            String b64_tab = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            var b64 = b64_tab.ToCharArray();
            String output = "";
            String container = "";
            String container2 = "";

            var input = data_in.ToString().ToCharArray();
            int padding = 0;
            
            char[] cont;
            StringBuilder bsf = new StringBuilder();
            int j = 0;
            int i = 0;
            bool coding = true;
            while (coding)
            {
                for (i = j; i < j + 24; i++)
                {
                    if (i < input.Length)
                    {
                        container = container + input[i];
                    }
                    else
                    {
                        padding = container.Length % 3;
                        if (padding != 0)
                        {
                            container = container.PadRight(24, '0');

                        }
                        coding = false;
                        break;
                    }
                }
                j = i;
                if (i == input.Length)
                {
                    coding = false;
                }
                cont = container.ToCharArray();
                container = "";
                if (padding == 0)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        for (int l = 0; l < 6; l++)
                        {
                            container2 = container2 + cont[l + 6 * k];
                        }
                        output = output + b64[Convert.ToInt32(container2, 2)];
                        container2 = "";
                    }
                }
                else
                {
                    if (padding != 0)
                    {
                        for (int k = 0; k < 4 - padding; k++)
                        {
                            for (int l = 0; l < 6; l++)
                            {
                                container2 = container2 + cont[l + 6 * k];
                            }
                            output = output + b64[Convert.ToInt32(container2, 2)];
                            container2 = "";
                        }
                        for (int k = 0; k < padding; k++)
                        {
                            output = output + "=";
                        }
                    }
                }

            }

            return output;
        }

        static String Base64Decoder(byte[] input64)
        {
            String b64_tab = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            var b64 = b64_tab.ToCharArray();
            String output = "";
            for (int i = 0; i < input64.Length; i++)
            {
                for (int j = 0; j < b64.Length; j++)
                {
                    if (input64[i] == Convert.ToInt32(b64[j]))
                    {
                        String temp = Convert.ToString(j, 2);
                        output = output + temp.PadLeft(6, '0');
                    }
                }
            }
            output = output.Remove(output.Length - output.Length % 8, output.Length % 8);
            return output;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Select mode : e - encode, d - decode ");
            string mode="";
            string file_path="";
            mode=Console.ReadLine();
            Console.WriteLine("Write file path");
            file_path = Console.ReadLine();
            if(mode=="e")
            {
                byte[] fileBytes = File.ReadAllBytes(file_path);
                StringBuilder sb = new StringBuilder();

                foreach (byte b in fileBytes)
                {
                    sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
                }

                var data = GetBytesFromBinaryString(sb.ToString());
                using (BinaryWriter writer = new BinaryWriter(File.Open(file_path, FileMode.Create)))
                {
                    writer.Write(data);
                }
                string file_path_out = file_path.Remove(file_path.Length-4,4)+"_out.b64";
                File.WriteAllText(file_path_out, Base64Encoder(sb.ToString()));
            }
            else if(mode == "d")
            {
            Console.WriteLine("Write output extension for example (.bmp)");
            string file_ext = Console.ReadLine();

            byte[] fileBytes = File.ReadAllBytes(file_path);
            StringBuilder sb64 = new StringBuilder();

            foreach (byte b in fileBytes)
            {
                sb64.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            var data64 = GetBytesFromBinaryString(sb64.ToString());
            var data64decoded = GetBytesFromBinaryString(Base64Decoder(data64));
            string file_path_out = file_path.Remove(file_path.Length - 4, 4) + "_out"+ file_ext;
            using (BinaryWriter writer = new BinaryWriter(File.Open(file_path_out, FileMode.Create)))
            {
                writer.Write(data64decoded);
            }
            }

            Console.WriteLine("Done, Press any key to close.");
            Console.ReadKey();
        }
    }
}
