using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//コメント衝突判定用のキャンバス転写コライダーの処理
public class PlayerColliderOnCanvas : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] float x, y, z;


    Camera cmrPublic, cmrUI;
    Rigidbody rigidbody;
    BoxCollider bc;

    // Start is called before the first frame update
    void Start()
    {
        cmrPublic = PlayingManager.playingManager.cmrPublic;
        cmrUI = PlayingManager.playingManager.cmrUI;
        rigidbody = Target.GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Target.gameObject.activeSelf)　//対象部位が有効時のみ。パラシュート用。非表示時はコメント衝突させないため
        {
            if (!bc.enabled)
            {
                bc.enabled = true;　//Boxコライダー有効化

            }
            var posViewportPublic = cmrPublic.WorldToViewportPoint(Target.position);　//Publicカメラ上での対象部位のビューポート座標
            var posWorldUi = cmrUI.ViewportToWorldPoint(posViewportPublic);　//ビューポート座標からUIカメラ基準のワールド座標（=cvsPulic用の座標）に変換。
            transform.position = posWorldUi;
            transform.localPosition = new Vector3(0f, transform.localPosition.y, transform.localPosition.z);　//念の為キャンバス平面上に移動（X軸が奥行き）
            transform.rotation = Target.rotation;
            transform.Rotate(x, y, z);　//対象部位のデフォルトの回転を相殺用
        }
        else
        {
            if (bc.enabled) bc.enabled = false;　//パラシュート用コライダーは開始時に無効化
        }
    }

    bool isHitBack = false;
    //コメント接触時
    private void OnTriggerEnter(Collider other)
    {
        //ブランコを漕いでいる最中（プレイ中）とジャンプ後でコメント衝突時の挙動を切り分け
        if (PlayingManager.playingManager.Stat == PlayingManager.Stat_global.play)　//プレイ中　
        {
            //右に移動時にコメント接触すると大きく減速してしまうため、プレイヤーの移動方向で力の大きさを変える
            //ただし右移動時の接触を完全に無効にすると「当たってる感」がなくなるので右移動時にも弱い力を掛ける
            //左移動時に接触したコメントは接触判定を失う
            //右移動時に接触したコメントは右移動時中のみ接触判定を失う
            if (rigidbody.velocity.z >= 0f)
            {
                rigidbody.AddForce(0f, 3f, 20f, ForceMode.Impulse);
                other.enabled = false;
            }
            else
            {
                if (!isHitBack)
                {
                    rigidbody.AddForce(0f, 0f, 5f, ForceMode.Impulse);
                    isHitBack = true;
                }
            }
        }
        if (PlayingManager.playingManager.Stat == PlayingManager.Stat_global.jump ||
            PlayingManager.playingManager.Stat == PlayingManager.Stat_global.fly)   //ジャンプ後
        {

            rigidbody.AddForce(0f, 0f, 30f, ForceMode.Impulse);
            other.enabled = false;
        }
    }

}
