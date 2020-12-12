using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

public static class Ini { //Штука, которая занимается записью сохраненных данных в файл

    //Статичный потому что мне лень создавать для него объекты - к этому классу сейчас можно просто напрямую обратиться откуда угодно

    private static ArrayList keys = new ArrayList(); //тут хранятся названия переменных
    private static ArrayList values = new ArrayList(); //тут хранятся их значения
    private static List<Header> headers = new List<Header>(); //эт для удобства чтения файла (естессна если он не зашифрован)

    static Cipher cipher = new Cipher(); //Шифратор для того, чтобы никакой злой дядька не изменил сохраненные данные

    public static bool FileExists(string way) //Метод для проверки существования файла
    {
        return File.Exists(Application.dataPath + "/" + way) ? true : false;
    }

    public static void ClearValues()
    {
        keys.Clear();
        values.Clear();
        headers.Clear();
    }

    public static void Set(string key, string value) //метод для записи переменной
    {
        for(int i = 0; i < keys.Count; i++)
        {
            if((string)keys[i] == key)
            {
                values[i] = value;
                return;
            }
        }

        keys.Add(key);
        values.Add(value);
    }

    public static void AddHeader(string text) //метод для добавления заголовка
    {
        headers.Add(new Header(keys.Count, text));
    }

    public static string Get(string key) //метод для вытаскивания значения переменной
    {
        for(int i = 0; i < keys.Count; i++)
        {
            if(keys[i].Equals(key))
            {
                return values[i].ToString();
            }
        }
        Debug.Log("переменная " + key + " не найдена");
        return "";
    }

    public static void Remove(string key) //хз зачем убирать переменную, но пусть будет, чо
    {
        for(int i = 0; i < keys.Count; i++)
        {
            if(keys[i].Equals(key))
            {
                keys.RemoveAt(i);
                values.RemoveAt(i);
            }
        }
        Debug.Log("Ключ не найден");
    }

    public static void Save(string way) //запись всего вместе в файл
    {
        int countHeader = 0; //Для подсчета заголовков 
        StreamWriter writer = new StreamWriter(Application.dataPath + "/" + way); //Запускаем нашего писателя

        for(int i = 0; i < keys.Count; i++)
        {
            if(countHeader < headers.Count && headers[countHeader].Line == i) //Если новая строка - заголовок
            {
                string newLine = "[" + headers[countHeader].Message + "]"; //То пишем заголовок
                writer.WriteLine(cipher.CompileLine(newLine)); //Вот так записывается зашифрованное сохранение
                //writer.WriteLine(newLine); //Вот так записывается незашифрованное сохранение (либо то, либо это)
                countHeader++; //И добавляем его в подсчет
            }
            string newestLine = keys[i] + ": " + values[i]; //При это все это - не "иначе", и выполняется даже если надо писать заголовок
            writer.WriteLine(cipher.CompileLine(newestLine)); //шифр
            //writer.WriteLine(newestLine); //не шифр
        }
        writer.Close();

        //Debug.Log("игра сохранена с:");
    }

    public static void LoadFile(string way) //Загрузку, признаюсь, частями слизал с инета, но эт довольно сложновато придумывать самому
    {
        string line = "", dir = Application.dataPath + "/" + way; //Тут мы создаем переменные для линии и пути ... наверно :\
        int offset = 0, lineCount = 0; //Оффсет - штука, разделяющая строку на ключ и значение 
        try //Если что-нибудь, что внутри трая, выдаст ошибку - начнет выполняться то, что написано в catch
        {
            using (StreamReader reader = new StreamReader(dir))
            {
                while ((line = cipher.DecompileLine(reader.ReadLine())) != null) //Вот так читается зашифрованное сохранение
                //while ((line = reader.ReadLine()) != null) //Вот так читается незашифорванное сохранение
                {
                    lineCount++;
                    if (!line.StartsWith("[")) //Если наша следующая линия - это не загловок
                    {
                        offset = line.IndexOf(":"); //То ставим разделитель на двоеточие
                        if(offset > 0)
                        {
                            Set(line.Substring(0, offset), line.Substring(offset + 2)); //И устанавливаем новую переменную  
                        }
                    }
                    else //Иначе создаем заголовок
                    {
                        offset = line.IndexOf("]");
                        AddHeader(line.Substring(1, offset));
                        
                    }
                }
                reader.Close();
            }
        }
        catch (IOException e)
        {
            Debug.Log("Что-то не то там у тебя с файлом крч - " + e);
        }
    }  
}

class Header //эта штука нужна для заголовков, логично
{
    public string Message;
    public int Line;

    public Header(int line, string message)
    {
        Message = message;
        Line = line;
    }
}

class Cipher //Шифратор крч (оч долго его писал, но оно точно стоит того)
{
    //AAAA = 0, AAAB = 1, AAAC = 2, AAAD = 3 ... 
    string ConvertTo(char value)
    {
        switch(value)
        {
            case '0':
                return "AAAA";
            case '1':
                return "AAAB";
            case '2':
                return "AAAC";
            case '3':
                return "AAAD";
            case '4':
                return "AABA";
            case '5':
                return "AABB";
            case '6':
                return "AABC";
            case '7':
                return "AABD";
            case '8':
                return "AACA";
            case '9':
                return "AACB";
            case '_':
                return "AACC";
            case 'A':
                return "AACD";
            case 'B':
                return "AADA";
            case 'C':
                return "AADB";
            case 'D':
                return "AADC";
            case 'E':
                return "AADD";
            case 'F':
                return "DDAA";
            case 'G':
                return "ABAA";
            case 'H':
                return "ABAB";
            case 'I':
                return "ABAC";
            case 'J':
                return "ABAD";
            case 'K':
                return "ABBA";
            case 'L':
                return "ABBB";
            case 'M':
                return "ABBC";
            case 'N':
                return "ABBD";
            case 'O':
                return "ABCA";
            case 'P':
                return "ABCB";
            case 'Q':
                return "ABCD";
            case 'R':
                return "ABDA";
            case 'S':
                return "ABDB";
            case 'T':
                return "ABDC";
            case 'U':
                return "ABDD";
            case 'V':
                return "ACAA";
            case 'W':
                return "ACAB";
            case 'X':
                return "ACAC";
            case 'Y':
                return "ACAD";
            case 'Z':
                return "ACBA";
            case 'a':
                return "ACBB";
            case 'b':
                return "ACBC";
            case 'c':
                return "ACBD";
            case 'd':
                return "ACCA";
            case 'e':
                return "ACCB";
            case 'f':
                return "ACCC";
            case 'g':
                return "ACCD";
            case 'h':
                return "ACDA";
            case 'i':
                return "ACDB";
            case 'j':
                return "ACDC";
            case 'k':
                return "ACDD";
            case 'l':
                return "ADAA";
            case 'm':
                return "ADAB";
            case 'n':
                return "ADAC";
            case 'o':
                return "ADAD";
            case 'p':
                return "ADBA";
            case 'q':
                return "ADBB";
            case 'r':
                return "ADBC";
            case 's':
                return "ADBD";
            case 't':
                return "ADCA";
            case 'u':
                return "ADCB";
            case 'v':
                return "ADCC";
            case 'w':
                return "ADCD";
            case 'x':
                return "ADDA";
            case 'y':
                return "ADDB";
            case 'z':
                return "ADDC";
            case ':':
                return "DADB";
            case '[':
                return "DADC";
            case ']':
                return "ADDD";
            case '.':
                return "BAAA";
            case '-':
                return "BAAB";
            case ' ':
                return "BAAC";
            case 'А':
                return "RAAA";
            case 'Б':
                return "RAAB";
            case 'В':
                return "RAAC";
            case 'Г':
                return "RAAD";
            case 'Д':
                return "RABA";
            case 'Е':
                return "RABB";
            case 'Ё':
                return "RABC";
            case 'Ж':
                return "RABD";
            case 'З':
                return "RACA";
            case 'И':
                return "RACB";
            case 'Й':
                return "RACC";
            case 'К':
                return "RACD";
            case 'Л':
                return "RADA";
            case 'М':
                return "RADB";
            case 'Н':
                return "RADC";
            case 'О':
                return "RADD";
            case 'П':
                return "RBAA";
            case 'Р':
                return "RBAB";
            case 'С':
                return "RBAC";
            case 'Т':
                return "RBAD";
            case 'У':
                return "RBBA";
            case 'Ф':
                return "RBBB";
            case 'Х':
                return "RBBC";
            case 'Ц':
                return "RBBD";
            case 'Ч':
                return "RBCA";
            case 'Ш':
                return "RBCB";
            case 'Щ':
                return "RBCC";
            case 'Ъ':
                return "RBCD";
            case 'Ы':
                return "RBDA";
            case 'Ь':
                return "RBDB";
            case 'Э':
                return "RBDC";
            case 'Ю':
                return "RBDD";
            case 'Я':
                return "RCAA";
            case 'а':
                return "RCAB";
            case 'б':
                return "RCAC";
            case 'в':
                return "RCAD";
            case 'г':
                return "RCBA";
            case 'д':
                return "RCBB";
            case 'е':
                return "RCBC";
            case 'ё':
                return "RCBD";
            case 'ж':
                return "RCCA";
            case 'з':
                return "RCCB";
            case 'и':
                return "RCCC";
            case 'й':
                return "RCCD";
            case 'к':
                return "RCDA";
            case 'л':
                return "RCDB";
            case 'м':
                return "RCDC";
            case 'н':
                return "RCDD";
            case 'о':
                return "RDAA";
            case 'п':
                return "RDAB";
            case 'р':
                return "RDAC";
            case 'с':
                return "RDAD";
            case 'т':
                return "RDBA";
            case 'у':
                return "RDBB";
            case 'ф':
                return "RDBC";
            case 'х':
                return "RDBD";
            case 'ц':
                return "RDCA";
            case 'ч':
                return "RDCB";
            case 'ш':
                return "RDCC";
            case 'щ':
                return "RDCD";
            case 'ъ':
                return "RDDA";
            case 'ы':
                return "RDDB";
            case 'ь':
                return "RDDC";
            case 'э':
                return "RDDD";
            case 'ю':
                return "RRAA";
            case 'я':
                return "RRAB";
            case '!':
                return "RRAC";
        } //Да, я все это писал вручную
        Debug.Log(value + " - Это что-то новое!");
        return "";
    }

    char ConvertUpTo(string value)
    {
        switch (value)
        {
            case "AAAA":
                return '0';
            case "AAAB":
                return '1';
            case "AAAC":
                return '2';
            case "AAAD":
                return '3';
            case "AABA":
                return '4';
            case "AABB":
                return '5';
            case "AABC":
                return '6';
            case "AABD":
                return '7';
            case "AACA":
                return '8';
            case "AACB":
                return '9';
            case "AACC":
                return '_';
            case "AACD":
                return 'A';
            case "AADA":
                return 'B';
            case "AADB":
                return 'C';
            case "AADC":
                return 'D';
            case "AADD":
                return 'E';
            case "DDAA":
                return 'F';
            case "ABAA":
                return 'G';
            case "ABAB":
                return 'H';
            case "ABAC":
                return 'I';
            case "ABAD":
                return 'J';
            case "ABBA":
                return 'K';
            case "ABBB":
                return 'L';
            case "ABBC":
                return 'M';
            case "ABBD":
                return 'N';
            case "ABCA":
                return 'O';
            case "ABCB":
                return 'P';
            case "ABCD":
                return 'Q';
            case "ABDA":
                return 'R';
            case "ABDB":
                return 'S';
            case "ABDC":
                return 'T';
            case "ABDD":
                return 'U';
            case "ACAA":
                return 'V';
            case "ACAB":
                return 'W';
            case "ACAC":
                return 'X';
            case "ACAD":
                return 'Y';
            case "ACBA":
                return 'Z';
            case "ACBB":
                return 'a';
            case "ACBC":
                return 'b';
            case "ACBD":
                return 'c';
            case "ACCA":
                return 'd';
            case "ACCB":
                return 'e';
            case "ACCC":
                return 'f';
            case "ACCD":
                return 'g';
            case "ACDA":
                return 'h';
            case "ACDB":
                return 'i';
            case "ACDC":
                return 'j';
            case "ACDD":
                return 'k';
            case "ADAA":
                return 'l';
            case "ADAB": 
                return 'm';
            case "ADAC":
                return 'n';
            case "ADAD":
                return 'o';
            case "ADBA":
                return 'p';
            case "ADBB":
                return 'q';
            case "ADBC":
                return 'r';
            case "ADBD":
                return 's';
            case "ADCA":
                return 't';
            case "ADCB":
                return 'u';
            case "ADCC":
                return 'v';
            case "ADCD":
                return 'w';
            case "ADDA":
                return 'x';
            case "ADDB":
                return 'y';
            case "ADDC":
                return 'z';
            case "DADB":
                return ':';
            case "DADC":
                return '[';
            case "ADDD":
                return ']';
            case "BAAA":
                return '.';
            case "BAAB":
                return '-';
            case "BAAC":
                return ' ';
            case "RAAA":
                return 'А';
            case "RAAB":
                return 'Б';
            case "RAAC":
                return 'В';
            case "RAAD":
                return 'Г';
            case "RABA":
                return 'Д';
            case "RABB":
                return 'Е';
            case "RABC":
                return 'Ё';
            case "RABD":
                return 'Ж';
            case "RACA":
                return 'З';
            case "RACB":
                return 'И';
            case "RACC":
                return 'Й';
            case "RACD":
                return 'К';
            case "RADA":
                return 'Л';
            case "RADB":
                return 'М';
            case "RADC":
                return 'Н';
            case "RADD":
                return 'О';
            case "RBAA":
                return 'П';
            case "RBAB":
                return 'Р';
            case "RBAC":
                return 'С';
            case "RBAD":
                return 'Т';
            case "RBBA":
                return 'У';
            case "RBBB":
                return 'Ф';
            case "RBBC":
                return 'Х';
            case "RBBD":
                return 'Ц';
            case "RBCA":
                return 'Ч';
            case "RBCB":
                return 'Ш';
            case "RBCC":
                return 'Щ';
            case "RBCD":
                return 'Ъ';
            case "RBDA":
                return 'Ы';
            case "RBDB":
                return 'Ь';
            case "RBDC":
                return 'Э';
            case "RBDD":
                return 'Ю';
            case "RCAA":
                return 'Я';
            case "RCAB":
                return 'а';
            case "RCAC":
                return 'б';
            case "RCAD":
                return 'в';
            case "RCBA":
                return 'г';
            case "RCBB":
                return 'д';
            case "RCBC":
                return 'е';
            case "RCBD":
                return 'ё';
            case "RCCA":
                return 'ж';
            case "RCCB":
                return 'з';
            case "RCCC":
                return 'и';
            case "RCCD":
                return 'й';
            case "RCDA":
                return 'к';
            case "RCDB":
                return 'л';
            case "RCDC":
                return 'м';
            case "RCDD":
                return 'н';
            case "RDAA":
                return 'о';
            case  "RDAB":
                return 'п';
            case "RDAC":
                return 'р';
            case "RDAD":
                return 'с';
            case "RDBA":
                return 'т';
            case "RDBB":
                return 'у';
            case "RDBC":
                return 'ф';
            case "RDBD":
                return 'х';
            case "RDCA":
                return 'ц';
            case "RDCB":
                return 'ч';
            case "RDCC":
                return 'ш';
            case "RDCD":
                return 'щ';
            case "RDDA":
                return 'ъ';
            case "RDDB":
                return 'ы';
            case  "RDDC":
                return 'ь';
            case "RDDD":
                return 'э';
            case "RRAA":
                return 'ю';
            case "RRAB":
                return 'я';
            case "RRAC":
                return '!';
        }
        Debug.Log(value + " - Что-то новое!");
        return '+';
    }

    public string CompileLine(string message)
    {
        char[] array = message.ToCharArray();
        StringBuilder builder = new StringBuilder();

        for(int i = 0; i < array.Length; i++)
        {
            builder.Append(ConvertTo(array[i]));
        }

        return builder.ToString();
    }

    public string DecompileLine(string message)
    {
        if (message != null && message.Length > 0)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < message.Length; i = i + 4)
            {
                builder.Append(ConvertUpTo(message.Substring(i, 4)));
            }

            return builder.ToString();
        }
        return null;
    }
} 