using UnityEngine;

//!	@brief	フィールドシーン
public class FieldScene : MonoBehaviour
{
	//!	@brief	開始処理
	//!	@return	なし
	void Start()
	{
		if( _playerModel != null ) {
			_player.create(new Player.PlayerCreateParam() {
				model	= _playerModel,
				anim	= _playerModel.GetComponent<Animator>(),
			});
			_player.initialize();
		}
	}
	
	//!	@brief	更新処理
	//!	@return	なし
	void Update()
	{
		_player.update();
	}

	[SerializeField] private GameObject		_playerModel	= null;				//!< プレイヤーモデル

	private Player							_player			= new Player();		//!< プレイヤー
}
