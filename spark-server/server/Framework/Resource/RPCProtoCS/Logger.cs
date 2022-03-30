// Generated by sprotodump. DO NOT EDIT!
// source: D:\csharpapps\SparkServer\spark-server\server\Framework\Tools\sproto2cs\..\\..\\Resource\\RPCProtoSchema\\Logger.sproto

using System;
using Sproto;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace NetSprotoType { 
	public class Logger_Init : SprotoTypeBase {
		private static int max_field_count = 1;
		
		
		private string _logger_path; // tag 0
		public string logger_path {
			get { return _logger_path; }
			set { base.has_field.set_field (0, true); _logger_path = value; }
		}
		public bool HasLogger_path {
			get { return base.has_field.has_field (0); }
		}

		public Logger_Init () : base(max_field_count) {}

		public Logger_Init (byte[] buffer) : base(max_field_count, buffer) {
			this.decode ();
		}

		protected override void decode () {
			int tag = -1;
			while (-1 != (tag = base.deserialize.read_tag ())) {
				switch (tag) {
				case 0:
					this.logger_path = base.deserialize.read_string ();
					break;
				default:
					base.deserialize.read_unknow_data ();
					break;
				}
			}
		}

		public override int encode (SprotoStream stream) {
			base.serialize.open (stream);

			if (base.has_field.has_field (0)) {
				base.serialize.write_string (this.logger_path, 0);
			}

			return base.serialize.close ();
		}
	}


}

