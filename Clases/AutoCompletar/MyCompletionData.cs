﻿// Copyright (c) 2009 Daniel Grunwald
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using My8086.Clases;
using My8086.Clases.AutoCompletar;
using My8086.Images;

namespace My8086
{
    /// <summary>
    /// Implements AvalonEdit ICompletionData interface to provide the entries in the completion drop down.
    /// </summary>
    public class MyCompletionData : ICompletionData
    {
        public MyCompletionData(string Token, string Completado, string Descripcion, int Orden, string ImgSource, AutoCompletado AutoCompletado)
        {
            this.Text = Completado;
            this.Descripcion = Descripcion;
            this.Orden = (double)Orden;
            this.ImgSource = ImgSource;
            this.ElementoAutoCompletado = new ElementoAutoCompletado(Token);
            this.AutoCompletado= AutoCompletado;
        }

        private readonly ElementoAutoCompletado ElementoAutoCompletado;
        public readonly string ImgSource;
        public System.Windows.Media.ImageSource Image
        {
            get => null;

        }

        public string Text { get; private set; }

        // Use this property if you want to show a fancy UIElement in the drop down list.
        public object Content => ElementoAutoCompletado;

        private readonly string Descripcion;
        public object Description
        {
            get => Descripcion;
        }

        private readonly double Orden;
        public double Priority
        {
            get => (double)Orden;
        }
        private readonly AutoCompletado AutoCompletado;

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, this.Text);

            DocumentLine linea = textArea.Document.GetLineByOffset(completionSegment.Offset);
            string contexto, tipo;
            switch (this.ImgSource)
            {
                case "Imgs\\AddVariable_16x.png":
                    contexto = textArea.Document.GetText(linea);
                    tipo = this.AutoCompletado.TiposDatos.FirstOrDefault(x => x.StartsWith(contexto.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (tipo != null)
                    {
                        textArea.Document.Replace(linea.Offset, linea.Length, Regex.Replace(contexto, tipo, tipo, RegexOptions.IgnoreCase));
                    }
                    break;
                case "Imgs\\Method_left_16x.png":
                    contexto = textArea.Document.GetText(linea);
                    tipo = this.AutoCompletado.Funciones.FirstOrDefault(x => contexto.ToLower().Trim().Contains(x.ToLower()));
                    if (tipo != null)
                    {
                        textArea.Document.Replace(linea.Offset, linea.Length, Regex.Replace(contexto, tipo, tipo, RegexOptions.IgnoreCase));
                    }
                    this.AutoCompletado.AutoCompletar();
                    break;
            }
            //GetText(completionSegment.Offset- this.Text.Length, this.Text.Length);
            //DocumentLine linea = this.Documento.GetLineByNumber((s as CompletionWindow).TextArea.Caret.Line);

        }
    }
}
