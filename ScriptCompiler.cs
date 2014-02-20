using System;
using System.Text;
using System.Collections;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Diagnostics;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System.IO;
using System.Windows.Forms;

namespace Assistant
{
	public class ScriptCompiler
	{
		private static Assembly m_Assembly;

		public static Assembly Assembly
		{
			get
			{
				return m_Assembly;
			}
			set
			{
				m_Assembly = value;
			}
		}

		public static string[] GetReferenceAssemblies()
		{
			ArrayList refs = new ArrayList( 1 );

			refs.Add( Engine.ExePath );

			string path = Path.Combine( Engine.BaseDirectory, "Scripts/References.cfg" );

			if ( File.Exists( path ) )
			{
				using ( StreamReader ip = new StreamReader( path ) )
				{
					string line;
					
					while ( (line = ip.ReadLine()) != null )
					{
						if ( line.Length > 0 && !line.StartsWith( "#" ) )
							refs.Add( line );
					}
				}
			}
			
			return (string[])refs.ToArray( typeof( string ) );
		}

		private static CompilerResults CompileCS( string[] files, string output )
		{
			//if ( files.Length == 0 )
			//	return null;
			CSharpCodeProvider provider = new CSharpCodeProvider();
			ICodeCompiler compiler = provider.CreateCompiler();

			CompilerResults results = compiler.CompileAssemblyFromFileBatch( new CompilerParameters( GetReferenceAssemblies(), output, false ), files );

			ProcessResults( results );

			return results;
		}

		private static void ProcessResults( CompilerResults results )
		{
			if ( results.Errors.Count > 0 )
			{
				int errorCount = 0, warningCount = 0;

				StringBuilder sb = new StringBuilder( "Compiling scripts... Results:\n" );
				foreach ( CompilerError e in results.Errors )
				{
					if ( e.IsWarning )
						++warningCount;
					else
						++errorCount;

					sb.AppendFormat( " - {0}: {1}: {2}: (line {3}, column {4}) {5}\n", e.IsWarning ? "Warning" : "Error", e.FileName, e.ErrorNumber, e.Line, e.Column, e.ErrorText );
				}
				sb.Append( "\n\n{0} warnings, {0} errors\n", warningCount, errorCount );

				if ( errorCount > 0 || warningCount > 0 )
				{
					sb.Replace( "\n", "\r\n" );
					MessageDialog dlg = new MessageDialog( "Script Compiler Message", errorCount == 0, sb.ToString() );
					dlg.ShowDialog();
					Process.GetCurrentProcess().Kill();
				}
			}
		}

		public static bool Compile()
		{
			EnsureDirectory( "Scripts" );
			EnsureDirectory( "Scripts/Compiled" );

			string[] files = GetScripts( "Scripts", "*.cs" );

			CompilerResults csResults = CompileCS( files, Path.Combine( Engine.BaseDirectory, "Scripts/Compiled/Scripts.dll" ) );
			
			if ( csResults == null || csResults.Errors.HasErrors )
				return false;

			m_Assembly = csResults.CompiledAssembly;
			InitializeAssembly( m_Assembly );
			InitializeAssembly( Assembly.GetCallingAssembly() );

			return true;
		}

		public static void InitializeAssembly( Assembly a )
		{
			Type[] types = a.GetTypes();

			for ( int i = 0; i < types.Length; ++i )
			{
				MethodInfo m = types[i].GetMethod( "Initialize", BindingFlags.Static | BindingFlags.Public );

				if ( m != null )
					m.Invoke( null, null );
			}
		}

		private static void EnsureDirectory( string dir )
		{
			string path = Path.Combine( Engine.BaseDirectory, dir );

			if ( !Directory.Exists( path ) )
				Directory.CreateDirectory( path );
		}

		private static string[] GetScripts( string baseDir, string type )
		{
			ArrayList list = new ArrayList();

			GetScripts( list, Path.Combine( Engine.BaseDirectory, baseDir ), type );

			return (string[])list.ToArray( typeof( string ) );
		}

		private static void GetScripts( ArrayList list, string path, string type )
		{
			foreach ( string dir in Directory.GetDirectories( path ) )
				GetScripts( list, dir, type );

			list.AddRange( Directory.GetFiles( path, type ) );
		}
	}
}
