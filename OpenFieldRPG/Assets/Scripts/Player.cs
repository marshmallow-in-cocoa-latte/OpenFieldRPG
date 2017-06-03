using UnityEngine;

public class Player
{
	//!	@brief	プレイヤー作成情報
	public struct PlayerCreateParam
	{
		public GameObject	model;		//!< モデル
		public Animator		anim;		//!< アニメーション制御
	}

	private const float				SP_STAY_ANIM_TIME		= 5.0f;									//!< 特殊待機モーションを再生するまでの時間
	private static readonly int		ANIM_TRIG_SP_STAY		= Animator.StringToHash("spStay");		//!< 特殊待機モーション再生アニメーショントリガー

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
		_stayTimer = SP_STAY_ANIM_TIME;
	}

	//!	@brief	更新
	//!	@return	なし
	public void update()
	{
		_stayTimer -= Time.deltaTime;
		if( _stayTimer < 0.0f ) {
			_stayTimer = SP_STAY_ANIM_TIME;
			setAnimTrigger(ANIM_TRIG_SP_STAY);
		}
	}

	//!	@brief	アニメーショントリガーの設定
	//!	@param	[in]	id		パラメーターID
	//!	@return	なし
	private void setAnimTrigger(int id)
	{
		if( _anim == null ) return;

		_anim.SetTrigger(id);
	}

	private GameObject		_model		= null;		//!< プレイヤーモデル
	private Animator		_anim		= null;		//!< アニメーション制御

	private float			_stayTimer	= 0.0f;		//!< 待機時間タイマー
}
