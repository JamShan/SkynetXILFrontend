// Generated by sprotodump. DO NOT EDIT!
// source: ../../Protos/proto.c2s.sproto

using System;
using Sproto;
using System.Collections.Generic;

namespace GameProtoC2SSprotoType { 
	public class login {
	
		public class request : SprotoTypeBase {
			private static int max_field_count = 2;
			
			
			private string _secret; // tag 0
			public string secret {
				get { return _secret; }
				set { base.has_field.set_field (0, true); _secret = value; }
			}
			public bool HasSecret {
				get { return base.has_field.has_field (0); }
			}

			private string _useid; // tag 1
			public string useid {
				get { return _useid; }
				set { base.has_field.set_field (1, true); _useid = value; }
			}
			public bool HasUseid {
				get { return base.has_field.has_field (1); }
			}

			public request () : base(max_field_count) {}

			public request (byte[] buffer) : base(max_field_count, buffer) {
				this.decode ();
			}

			protected override void decode () {
				int tag = -1;
				while (-1 != (tag = base.deserialize.read_tag ())) {
					switch (tag) {
					case 0:
						this.secret = base.deserialize.read_string ();
						break;
					case 1:
						this.useid = base.deserialize.read_string ();
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
					base.serialize.write_string (this.secret, 0);
				}

				if (base.has_field.has_field (1)) {
					base.serialize.write_string (this.useid, 1);
				}

				return base.serialize.close ();
			}
		}


		public class response : SprotoTypeBase {
			private static int max_field_count = 1;
			
			
			private Int64 _res; // tag 0
			public Int64 res {
				get { return _res; }
				set { base.has_field.set_field (0, true); _res = value; }
			}
			public bool HasRes {
				get { return base.has_field.has_field (0); }
			}

			public response () : base(max_field_count) {}

			public response (byte[] buffer) : base(max_field_count, buffer) {
				this.decode ();
			}

			protected override void decode () {
				int tag = -1;
				while (-1 != (tag = base.deserialize.read_tag ())) {
					switch (tag) {
					case 0:
						this.res = base.deserialize.read_integer ();
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
					base.serialize.write_integer (this.res, 0);
				}

				return base.serialize.close ();
			}
		}


	}


	public class package : SprotoTypeBase {
		private static int max_field_count = 3;
		
		
		private Int64 _type; // tag 0
		public Int64 type {
			get { return _type; }
			set { base.has_field.set_field (0, true); _type = value; }
		}
		public bool HasType {
			get { return base.has_field.has_field (0); }
		}

		private Int64 _session; // tag 1
		public Int64 session {
			get { return _session; }
			set { base.has_field.set_field (1, true); _session = value; }
		}
		public bool HasSession {
			get { return base.has_field.has_field (1); }
		}

		private string _ud; // tag 2
		public string ud {
			get { return _ud; }
			set { base.has_field.set_field (2, true); _ud = value; }
		}
		public bool HasUd {
			get { return base.has_field.has_field (2); }
		}

		public package () : base(max_field_count) {}

		public package (byte[] buffer) : base(max_field_count, buffer) {
			this.decode ();
		}

		protected override void decode () {
			int tag = -1;
			while (-1 != (tag = base.deserialize.read_tag ())) {
				switch (tag) {
				case 0:
					this.type = base.deserialize.read_integer ();
					break;
				case 1:
					this.session = base.deserialize.read_integer ();
					break;
				case 2:
					this.ud = base.deserialize.read_string ();
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
				base.serialize.write_integer (this.type, 0);
			}

			if (base.has_field.has_field (1)) {
				base.serialize.write_integer (this.session, 1);
			}

			if (base.has_field.has_field (2)) {
				base.serialize.write_string (this.ud, 2);
			}

			return base.serialize.close ();
		}
	}


}


public class GameProtoC2SProtocol : ProtocolBase {
	public static  GameProtoC2SProtocol Instance = new GameProtoC2SProtocol();
	private GameProtoC2SProtocol() {
		Protocol.SetProtocol<login> (login.Tag);
		Protocol.SetRequest<GameProtoC2SSprotoType.login.request> (login.Tag);
		Protocol.SetResponse<GameProtoC2SSprotoType.login.response> (login.Tag);

		Protocol.SetProtocol<ping> (ping.Tag);

	}

	public class login {
		public const int Tag = 2;
	}

	public class ping {
		public const int Tag = 1;
	}

}