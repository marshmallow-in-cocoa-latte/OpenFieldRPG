using UnityEngine;
using NUnit.Framework;
using System.Text;

//!	@brief	FSMテスト
public class FSMTest
{
	//!	@brief	テストステート
	private class FSMTestState : IFSMState
	{
		public string	name	{ get; }		//!< ステート名

		//!	@brief	コンストラクタ
		//!	@param	[in]	name	ステート名
		//!	@param	[in]	log		ログ
		public FSMTestState(string name, StringBuilder log)
		{
			this.name	= name;
			_log		= log;
		}

		//!	@brief	開始処理
		//!	@return	なし
		public void entry()
		{
			if( _log == null ) return;

			_log.AppendLine(name + ".entry");
		}

		//!	@brief	実行処理
		//!	@param	[in]	fsm		ステートマシン
		//!	@return	なし
		public void execute(FSMSystem fsm)
		{
			if( _log == null ) return;

			_log.AppendLine(name + ".exec");
		}

		//!	@brief	終了処理
		//!	@return	なし
		public void exit()
		{
			if( _log == null ) return;

			_log.AppendLine(name + ".exit");
		}

		private readonly StringBuilder		_log		= null;		//!< ログ
	}

	//!	@brief	テストステート
	private class FSMTestStateToNext : IFSMState
	{
		public string	name	{ get; }		//!< ステート名

		//!	@brief	コンストラクタ
		//!	@param	[in]	name	ステート名
		//!	@param	[in]	log		ログ
		public FSMTestStateToNext(string name, StringBuilder log)
		{
			this.name	= name;
			_log		= log;
		}

		//!	@brief	開始処理
		//!	@return	なし
		public void entry()
		{
			if( _log == null ) return;

			_log.AppendLine(name + ".entry");
		}

		//!	@brief	実行処理
		//!	@param	[in]	fsm		ステートマシン
		//!	@return	なし
		public void execute(FSMSystem fsm)
		{
			if( _log == null ) return;

			_log.AppendLine(name + ".exec");
			fsm.next();
		}

		//!	@brief	終了処理
		//!	@return	なし
		public void exit()
		{
			if( _log == null ) return;

			_log.AppendLine(name + ".exit");
		}

		private readonly StringBuilder		_log		= null;		//!< ログ
	}

	//!	@brief	ステート
	private static class State
	{
		public static string	Test0	{ get { return "test0"; } }		//!< テストステート0
		public static string	Test1	{ get { return "test1"; } }		//!< テストステート1
		public static string	Test2	{ get { return "test2"; } }		//!< テストステート2
	}

	//!	@brief	FSMシステムテスト
	//!	@return	なし
	[Test] public void FSMSystemTest()
	{
		var log = new StringBuilder();
		var fsm = new FSMSystem();

		fsm.initialize(new IFSMState[] {
			new FSMTestState(State.Test0, log),
			new FSMTestStateToNext(State.Test1, log),
			new FSMTestState(State.Test2, log),
		});

		fsm.execute();

		log.AppendLine("setNextState(" + State.Test0 + ")");
		fsm.setNextState(State.Test0);
		fsm.execute();

		log.AppendLine("setNextState(" + State.Test1 + ")");
		fsm.setNextState(State.Test1);
		fsm.execute();

		fsm.execute();

		var testResult = new StringBuilder()
			.AppendLine(State.Test0 + ".entry")
			.AppendLine(State.Test0 + ".exec")
			.AppendLine("setNextState(" + State.Test0 + ")")
			.AppendLine(State.Test0 + ".exec")
			.AppendLine("setNextState(" + State.Test1 + ")")
			.AppendLine(State.Test0 + ".exit")
			.AppendLine(State.Test1 + ".entry")
			.AppendLine(State.Test1 + ".exec")
			.AppendLine(State.Test1 + ".exit")
			.AppendLine(State.Test2 + ".entry")
			.AppendLine(State.Test2 + ".exec")
			;

		Assert.AreEqual(testResult.ToString(), log.ToString());
	}

	//!	@brief	FSMシステム初期化失敗テスト
	//!	@return	なし
	[Test] public void FSMSysmteInitializeFailedTest()
	{
		var log = string.Empty;
		var logMsgRecieved = new Application.LogCallback((condition, stackTrace, type) => {
			log = condition;
		});

		Application.logMessageReceived += logMsgRecieved;

		var fsm = new FSMSystem();
		fsm.initialize(new IFSMState[] {
			new FSMTestState(State.Test0, null),
			new FSMTestState(State.Test0, null),
		});

		Application.logMessageReceived -= logMsgRecieved;

		var testLog = 
			"State \"" + State.Test0 + "\" is duplicate !!!!!"
			+ "\nFailed initialize FSMSystem ..."
			;
		Assert.AreEqual(testLog, log);
	}
}
