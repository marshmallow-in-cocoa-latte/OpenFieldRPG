
//!	@brief	ステートインターフェイス
public interface IFSMState
{
	string	name	{ get; }		//!< ステート名

	//!	@brief	開始処理
	//!	@return	なし
	void entry();

	//!	@brief	実行処理
	//!	@param	[in]	fsm		ステートマシン
	//!	@return	なし
	void execute(FSMSystem fsm);

	//!	@brief	終了処理
	//!	@return	なし
	void exit();
}
