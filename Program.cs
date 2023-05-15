﻿using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;


namespace CPU {
    public class CPU {
        public UInt16 A;
        public UInt16 B;
        public UInt16[] M = new UInt16[65536];

        public int clock = 1000;

        public static void Main(string[] args) {
            CPU cpu = new CPU();

            bool show;
            Console.Write("Show Register and PC? (y / n): ");
            string nn = Console.ReadLine();

            Console.Clear();

            if (nn == "y" || nn == "Y")
                show = true;
            else
                show = false;

            string[] x = File.ReadAllLines("input.txt");

            for (int i = 0; i < x.Length;) {
                int check = 0;
                bool trigger = true;

                if (x[i] == "") {
                    trigger = false;
                    check = 1;
                }

                if (x[i] == "NOP") {
                    if (show == true)
                        Console.WriteLine("NOP");
                    check = 1;
                }

                if (x[i] == "MMV") {
                    if (show == true)
                        Console.WriteLine("MMV");
                    UInt16 temp1 = Convert.ToUInt16(x[i + 1]);
                    UInt16 temp2 = Convert.ToUInt16(x[i + 2]);

                    cpu.M[temp2] = cpu.M[temp1];

                    i = i + 2;
                    check = 1;
                }

                if (x[i] == "MOV") {
                    if (show == true)
                        Console.WriteLine("MOV");
                    string temp = x[i + 1];

                    if (temp == "A")
                        cpu.B = cpu.A;
                    if (temp == "B")
                        cpu.A = cpu.B;

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "LDA") {
                    if (show == true)
                        Console.WriteLine("LDA");
                    UInt16 temp1 = Convert.ToUInt16(x[i + 1]);

                    cpu.M[temp1] = cpu.A;

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "LDB") {
                    if (show == true)
                        Console.WriteLine("LDB");
                    UInt16 temp1 = Convert.ToUInt16(x[i + 1]);

                    cpu.M[temp1] = cpu.B;

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "WTA") {
                    if (show == true)
                        Console.WriteLine("WTA");
                    UInt16 temp1 = Convert.ToUInt16(x[i + 1]);

                    cpu.A = cpu.M[temp1];

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "WTB") {
                    if (show == true)
                        Console.WriteLine("WTB");
                    UInt16 temp1 = Convert.ToUInt16(x[i + 1]);

                    cpu.B = cpu.M[temp1];

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "ADD") {
                    if (show == true)
                        Console.WriteLine("ADD");
                    int temp = cpu.A + cpu.B;
                    cpu.A = Convert.ToUInt16(temp);
                    check = 1;
                }

                if (x[i] == "SUB") {
                    if (show == true)
                        Console.WriteLine("SUB");
                    int temp = cpu.A - cpu.B;
                    cpu.A = Convert.ToUInt16(temp);
                    check = 1;
                }

                if (x[i] == "MUL") {
                    if (show == true)
                        Console.WriteLine("MUL");
                    int temp = cpu.A * cpu.B;
                    cpu.A = Convert.ToUInt16(temp);
                    check = 1;
                }

                if (x[i] == "JMP") {
                    if (show == true)
                        Console.WriteLine("JMP");
                    i = Convert.ToUInt16(x[i + 1]);
                    check = 1;
                }

                if (x[i] == "CMP") {
                    if (show == true)
                        Console.WriteLine("CMP");
                    if (cpu.A == cpu.B)
                        cpu.A = 0;

                    if (cpu.A > cpu.B)
                        cpu.A = 1;

                    if (cpu.A < cpu.B)
                        cpu.A = 2;
                    check = 1;
                }

                if (x[i] == "JNE") {
                    if (show == true)
                        Console.WriteLine("JNE");
                    UInt16 temp = Convert.ToUInt16(x[i + 1]);

                    if (cpu.A != cpu.B)
                        i = temp;

                    else
                        i = i + 1;
                    check = 1;
                }

                if (x[i] == "JIE") {
                    if (show == true)
                        Console.WriteLine("JIE");
                    UInt16 temp = Convert.ToUInt16(x[i + 1]);

                    if (cpu.A == cpu.B)
                        i = temp;

                    else
                        i = i + 1;
                    check = 1;
                }

                if (x[i] == "HLT") {
                    if (show == true)
                        Console.WriteLine("HLT");

                    if (show == true) {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("Register A: ");
                        Console.Write("Hex " + "{0:X4}", cpu.A);
                        Console.WriteLine(" | Decimal " + "{0:00000}", cpu.A);

                        Console.Write("Register B: ");
                        Console.Write("Hex " + "{0:X4}", cpu.B);
                        Console.WriteLine(" | Decimal " + "{0:00000}", cpu.B);

                        Console.Write("Memory ADR: ");
                        Console.Write("Hex " + "{0:X4}", i);
                        Console.WriteLine(" | Decimal " + "{0:00000}", i);

                        Console.Write("\n");

                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    while (true) ;
                }

                if (x[i] == "CRB") {
                    if (show == true)
                        Console.WriteLine("CRB");
                    UInt16 temp = Convert.ToUInt16(x[i + 1]);

                    cpu.B = temp;

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "call print") {
                    Console.Write(x[i + 1]);

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "call println") {
                    Console.WriteLine(x[i + 1]);

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "call printm") {
                    UInt16 temp1 = Convert.ToUInt16(x[i + 1]);
                    char temp2;

                    for (UInt16 t = temp1; cpu.M[t] != 0; t++) {
                        temp2 = Convert.ToChar(cpu.M[t]);
                        Console.Write(temp2);
                    }

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "call printlm") {
                    UInt16 temp1 = Convert.ToUInt16(x[i + 1]);
                    char temp2;

                    for (UInt16 t = temp1; cpu.M[t] != 0; t++) {
                        temp2 = Convert.ToChar(cpu.M[t]);
                        Console.Write(temp2);
                    }

                    Console.Write("\n");

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "call scanf") {
                    string temp = Console.ReadLine();
                    UInt16 temp2 = Convert.ToUInt16(x[i + 1]);

                    byte[] asciiBytes = Encoding.ASCII.GetBytes(temp);
                    int[] asciiInts = Array.ConvertAll(asciiBytes, b => (int)b);

                    for (int t = 0; t < asciiInts.Length; t++) {
                        cpu.M[t + temp2] = Convert.ToUInt16(asciiInts[t]);
                    }

                    i = i + 1;
                    check = 1;
                }

                i = i + 1;

                if (check == 0) {
                    Console.WriteLine("\nFalse assembly!");
                    Console.WriteLine("Program counter: " + i);
                    while (true) ;
                }

                if (show == true && trigger == true) {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Register A: ");
                    Console.Write("Hex " + "{0:X4}", cpu.A);
                    Console.WriteLine(" | Decimal " + "{0:00000}", cpu.A);

                    Console.Write("Register B: ");
                    Console.Write("Hex " + "{0:X4}", cpu.B);
                    Console.WriteLine(" | Decimal " + "{0:00000}", cpu.B);

                    Console.Write("Memory ADR: ");
                    Console.Write("Hex " + "{0:X4}", i);
                    Console.WriteLine(" | Decimal " + "{0:00000}", i);

                    Console.Write("\n");

                    Console.ForegroundColor = ConsoleColor.White;
                }

                Thread.Sleep(1000 / cpu.clock);
            }
        }
    }
}