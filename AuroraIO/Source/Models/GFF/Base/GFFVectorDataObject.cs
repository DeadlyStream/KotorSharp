using AuroraIO.Source.Models.GFF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO {
    public class GFFVectorDataObject : GFFFieldDataObject {

        public GFFFieldType fieldType() {
            return GFFFieldType.VECTOR;
        }

        public int subItemCount() {
            return 0;
        }

        public float x;
        public float y;
        public float z;

        public GFFVectorDataObject(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public GFFVectorDataObject(double x, double y, double z) {
            this.x = (float)x;
            this.y = (float)y;
            this.z = (float)z;
        }

        public void setValueForPath(object value, GFFPath path) {
            GFFPath firstPathComponent = path.first();
            GFFPath remainingPath = path.removingFirst();
            String lastPathComponent = path.last();

            float floatValue = 0.0f;
            if (value is float) {
                floatValue = (float)value;
            } else if (value is String) {
                var stringValue = value as String;
                floatValue = float.Parse(stringValue);
            }

            if (firstPathComponent.Equals("x")) {
                this.x = floatValue;
            } else if (firstPathComponent.Equals("y")) {
                this.y = floatValue;
            } else if (firstPathComponent.Equals("z")) {
                this.z = floatValue;
            }
        }

        public Object getValueAtPath(GFFPath path) {
            GFFPath firstPathComponent = path.first();
            GFFPath remainingPath = path.removingFirst();
            String lastPathComponent = path.last();

            if (firstPathComponent.Equals("x")) {
                return x;
            } else if (firstPathComponent.Equals("y")) {
                return y;
            } else if (firstPathComponent.Equals("z")) {
                return z;
            } else {
                return null;
            }
        }

        public int dataLength() {
            return 12;
        }

        public byte[] toBytes() {
            ByteArray newByteArray = new ByteArray();
            newByteArray.AddRange(BitConverter.GetBytes(x));
            newByteArray.AddRange(BitConverter.GetBytes(y));
            newByteArray.AddRange(BitConverter.GetBytes(z));
            return newByteArray.ToArray();
        }

        public override string ToString() {
            return String.Format("{{x: {0}, y: {1}, z: {2}}}", x, y, z);
        }

        public string asciiEncoding(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("value");
            sb.AppendLine(String.Format("x {0}", x));
            sb.AppendLine(String.Format("y {0}", y));
            sb.AppendLine(String.Format("z {0}", z));
            return sb.ToString();
        }
    }
}
