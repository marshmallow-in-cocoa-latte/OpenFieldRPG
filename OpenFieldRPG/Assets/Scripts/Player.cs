using UnityEngine;

//!	@brief	プレイヤー
public class Player
{
	//!	@brief	プレイヤー作成情報
	public struct PlayerCreateParam
	{
		public GameObject	model;		//!< モデル
		public Animator		anim;		//!< アニメーション制御
	}

	//!	@brief	待機ステート
	private class StayState : IFSMState
	{
		private const float				SP_STAY_ANIM_TIME		= 5.0f;									//!< 特殊待機モーションを再生するまでの時間
		private static readonly int		ANIM_TRIG_SP_STAY		= Animator.StringToHash("spStay");		//!< 特殊待機モーション再生アニメーショントリガー

		public string	name	{ get; }		//!< ステート名

		//!	@brief	コンストラクタ
		//!	@param	[in]	name	ステート名
		//!	@param	[in]	player	プレイヤー
		public StayState(string name, Player player)
		{
			this.name	= name;
			_player		= player;
		}

		//!	@brief	開始処理
		//!	@return	なし
		public void entry()
		{
			_stayTimer = SP_STAY_ANIM_TIME;
		}

		//!	@brief	実行処理
		//!	@param	[in]	fsm		ステートマシン
		//!	@return	なし
		public void execute(FSMSystem fsm)
		{
			_stayTimer -= Time.deltaTime;
			if( _stayTimer > 0.0f ) return;

			if( _player != null ) {
				_player.setAnimTrigger(ANIM_TRIG_SP_STAY);
			}
			_stayTimer = SP_STAY_ANIM_TIME;
		}

		//!	@brief	終了処理
		//!	@return	なし
		public void exit()
		{
		}

		private Player	_player		= null;		//!< プレイヤー
		private float	_stayTimer	= 0.0f;		//!< 待機時間タイマー
	}

	//!	@brief	ステート
	private static class State
	{
		public static string	Stay	{ get { return "stay"; } }
	}

	//!	@brief	コンストラクタ
	public Player()
	{
		_fsm.initialize(new IFSMState[] {
			new StayState(State.Stay, this),
		});
	}

	//!	@brief	作成
	//!	@param	[in]	param	プレイヤー作成情報
	//!	@return	なし
	public void create(PlayerCreateParam param)
	{
		_model	= param.model;
		_anim	= param.anim;
	}

	//!	@brief	初期化
	//!	@return	なし
	public void initialize()
	{
	}

	//!	@brief	更新
	//!	@return	なし
	public void update()
	{
		_fsm.execute();
	}

	//!	@brief	アニメーショントリガーの設定
	//!	@param	[in]	id		パラメーターID
	//!	@return	なし
	private void setAnimTrigger(int id)
	{
		if( _anim == null ) return;

		_anim.SetTrigger(id);
	}

	private GameObject		_model		= null;					//!< プレイヤーモデル
	private Animator		_anim		= null;					//!< アニメーション制御

	private FSMSystem		_fsm		= new FSMSystem();		//!< ステートマシン
}
