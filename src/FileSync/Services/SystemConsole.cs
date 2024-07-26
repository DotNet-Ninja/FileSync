namespace FileSync.Services;

public class SystemConsole : IConsole
{
    public ConsoleColor ErrorColor { get; set; } = ConsoleColor.DarkRed;
    public ConsoleColor WarningColor { get; set; } = ConsoleColor.Yellow;

    public void Write(char character)
    {
        Console.Write(character);
    }

    public void Write(string message)
    {
        Console.Write(message);
    }

    public void Write(string template, params object[] parameters)
    {
        Console.Write(template, parameters);
    }

    public void Write(ConsoleColor color, string message)
    {
        var tmp = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Write(message);
        Console.ForegroundColor = tmp;
    }

    public void Write(ConsoleColor color, string template, params object[] parameters)
    {
        var tmp = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Write(template, parameters);
        Console.ForegroundColor = tmp;
    }

    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }

    public void WriteLine(string template, params object[] parameters)
    {
        Console.WriteLine(template, parameters);
    }

    public void WriteLine()
    {
        Console.WriteLine();
    }

    public void WriteLine(ConsoleColor color, string message)
    {
        var tmp = Console.ForegroundColor;
        Console.ForegroundColor = color;
        WriteLine(message);
        Console.ForegroundColor = tmp;
    }

    public void WriteLine(ConsoleColor color, string template, params object[] parameters)
    {
        var tmp = Console.ForegroundColor;
        Console.ForegroundColor = color;
        WriteLine(template, parameters);
        Console.ForegroundColor = tmp;
    }

    public void WriteWarning(string message)
    {
        WriteLine(WarningColor, message);
    }

    public void WriteWarning(string template, params object[] parameters)
    {
        WriteLine(WarningColor, template, parameters);
    }

    public void WriteError(string message)
    {
        WriteLine(ErrorColor, message);
    }

    public void WriteError(string template, params object[] parameters)
    {
        WriteLine(ErrorColor, template, parameters);
    }

    public void WriteError(Exception exception)
    {
        var current = exception;
        var prefix = "Exception";
        while (current is not null)
        {
            WriteError(prefix);
            WriteError(current.Message);
            WriteError(exception.StackTrace ?? string.Empty);
            current = current.InnerException;
            prefix = "Inner Exception";
        }
    }

    public void WriteError(Exception exception, string message)
    {
        WriteError(message);
        WriteError(exception);
    }

    public byte[] ReadSecure4(string prompt, char pwdChar = '*')
    {
        WriteLine(prompt);
        ConsoleKeyInfo key;
        var password = new byte[256];
        var index = 1;
        do
        {
            key = Console.ReadKey(true);
            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                password[index]= (byte)(((byte)key.KeyChar) + password[0]);
                Write(pwdChar);
                index++;
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                index--;
                password[index]=0;
                Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter && index<=256);
        WriteLine();
        var current = 1;
        var length = 0;
        while (current != 0 && length<=password.Length)
        {
            current = password[length];
            length++;
        }

        var result = new byte[length-2];
        Array.Copy(password, 1, result, 0, length-2);
        for (int i = 0; i < password.Length; i++)
        {
            password[i] = 0;
        }

        return result;
    }

    public byte[] ReadSecure(string prompt = "Enter Password", char pwdChar = '*')
    {
        const int maxLength = 96;
        WriteLine($"{prompt}:");
        
        ConsoleKeyInfo input;
        var password = new byte[maxLength];
        var index = 0;
        do
        {
            input = Console.ReadKey(true);
            if (input.Key != ConsoleKey.Backspace && input.Key != ConsoleKey.Enter)
            {
                password[index] = (byte)(((byte)input.KeyChar));
                Write(pwdChar);
                index++;
            }
            else if (input.Key == ConsoleKey.Backspace && index > 0)
            {
                index--;
                password[index] = 0;
                Write("\b \b");
            }
        } while (input.Key != ConsoleKey.Enter && index <= maxLength);

        var result = new byte[index];
        Array.Copy(password, 0, result, 0, index);
        for (int i = 0; i < password.Length; i++)
        {
            password[i] = 0;
        }
        WriteLine();
        return result;
    }
}