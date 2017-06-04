using UnityEngine;
using System.Collections.Generic;

//!	@brief	ステートマシン
public class FSMSystem
{
	private const int	INVALID_STATE_INDEX		= -1;	//!< 無効なステートインデックス

	//!	@brief	初期化
	//!	@param	[in]	states	ステート
	//!	@return	なし
	public void initialize(IFSMState[] states)
	{
		clear();

		if( states == null 
			|| states.Length <= 0
			) return;

		_states = states;
		for( var i = 0; i < _states.Length; ++i ) {
			var state = _states[i];
			if( state == null ) continue;

			var stateName = state.name;
			if( _stateIndexTable.ContainsKey(stateName) ) {
				Debug.LogWarning(
					"State \"" + stateName + "\" is duplicate !!!!!"
					+ "\nFailed initialize FSMSystem ..."
					);
				clear();
				return;
			}

			_stateIndexTable.Add(stateName, i);
		}

		_curStateIndex	= 0;
		_nextStateIndex	= 0;
		_isEntry		= false;
	}

	//!	@brief	クリア
	//!	@return	なし
	public void clear()
	{
		_stateIndexTable.Clear();
		_states = null;

		_curStateIndex	= INVALID_STATE_INDEX;
		_nextStateIndex	= INVALID_STATE_INDEX;
	}

	//!	@brief	実行
	//!	@return	なし
	public void execute()
	{
		var state = getState(_curStateIndex);
		if( _curStateIndex != _nextStateIndex ) {
			if( state != null ) {
				state.exit();
			}

			_curStateIndex = _nextStateIndex;
			state = getState(_curStateIndex);
			_isEntry = false;
		}

		if( !_isEntry
			&& state != null
			) {
			state.entry();
			_isEntry = true;
		}

		if( state != null ) {
			state.execute(this);
		}

		if( _curStateIndex != _nextStateIndex ) {
			if( state != null ) {
				state.exit();
			}
			_curStateIndex = _nextStateIndex;
			_isEntry = false;
		}
	}

	//!	@brief	次のステートの設定
	//!	@param	[in]	name	ステート名
	//!	@return	なし
	public void setNextState(string name)
	{
		if( _states == null ) return;

		var nextStateIndex = INVALID_STATE_INDEX;
		if( !_stateIndexTable.TryGetValue(name, out nextStateIndex) ) return;

		_nextStateIndex = nextStateIndex;
	}

	//!	@brief	次のステートへ
	//!	@return	なし
	public void next()
	{
		if( _states == null ) return;

		_nextStateIndex = Mathf.Min(_curStateIndex + 1, _states.Length);
	}

	//!	@brief	前のステートへ
	//!	@return	なし
	public void prev()
	{
		if( _states == null ) return;

		_nextStateIndex = Mathf.Max(_curStateIndex - 1, 0);
	}

	//!	@brief	ステートの取得
	//!	@param	[in]	stateIndex		ステートインデックス
	//!	@return	ステート
	private IFSMState getState(int stateIndex)
	{
		if( _states == null
			|| stateIndex < 0
			|| stateIndex >= _states.Length
			) return null;

		return _states[stateIndex];
	}

	private Dictionary<string, int>		_stateIndexTable	= new Dictionary<string, int>();		//!< ステートからステートインデックスへのテーブル
	private IFSMState[]					_states				= null;									//!< ステート

	private int							_curStateIndex		= INVALID_STATE_INDEX;					//!< 現在のステートインデックス
	private int							_nextStateIndex		= INVALID_STATE_INDEX;					//!< 次のステートインデックス
	private bool						_isEntry			= false;								//!< 現在のステートを開始したか
}
