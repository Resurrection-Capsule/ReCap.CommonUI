// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Copyright (c) 2006 Novell, Inc. (https://www.novell.com)
//
//

using System;
using System.Collections.Generic;

namespace ReCap.CommonUI.Util.OperatingSystem.Linux
{
    internal partial class X11Atoms
    {
        readonly IntPtr _display;
        readonly Dictionary<string, IntPtr> _namesToAtoms = new Dictionary<string, IntPtr>();
        readonly Dictionary<IntPtr, string> _atomsToNames = new Dictionary<IntPtr, string>();

        internal void InitAtom(ref IntPtr field, string name, IntPtr value)
        {
            if (value != IntPtr.Zero)
            {
                field = value;
                _namesToAtoms[name] = value;
                _atomsToNames[value] = name;
            }
        }

        public IntPtr GetAtom(string name)
        {
            if (_namesToAtoms.TryGetValue(name, out var rv))
                return rv;
            var atom = LibX11.XInternAtom(_display, name, false);
            _namesToAtoms[name] = atom;
            _atomsToNames[atom] = name;
            return atom;
        }

        public string GetAtomName(IntPtr atom)
        {
            if (_atomsToNames.TryGetValue(atom, out var rv))
                return rv;
            var name = X11Helper.GetAtomName(_display, atom);
            if (name == null)
                return null;
            _atomsToNames[atom] = name;
            _namesToAtoms[name] = atom;
            return name;
        }
    }
}