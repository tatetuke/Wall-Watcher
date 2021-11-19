using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopMessageManager : MonoBehaviour
{
    public GameObject messageTextObj;
    public GameObject Manager;
    public Text yesText;
    public Text noText;
    [SerializeField] private Text messageText;
    public ShopSelectItemManager selectItemManager;
    public ShopBuyItem buyItem;
    private bool canGenerateDialog;
    private bool selectDialog;
    Color gray;
    Color black;
    enum ShopMode
    {
        buy,
        sell,
        upgrade
    }
    private ShopMode mode;

    // Start is called before the first frame update
    void Start()
    {
        canGenerateDialog = true;
        mode = ShopMode.buy;
        gray = noText.color;
        black = yesText.color;
    }

    // Update is called once per frame
    void Update()
    {
        InputBuyManager();
    }

    private void  InputBuyManager()
    {
        if (selectItemManager.item == null) return;
        //購買キーが押された時，買うかどうかのメッセージを表示する
        if (!Input.GetKeyDown("v"))return;
        //既にメッセージが表示されている場合は何もしない
        if (!canGenerateDialog) return;
        //買うかどうかのメッセージを表示する
        StartCoroutine(DaialogSelectManager());

    }
    IEnumerator DaialogSelectManager()
    {
        canGenerateDialog = false;
        //テキスト初期化
        TextInit();
        //アクティブ化
        messageTextObj.SetActive(true);
        //連続での入力を避けるために1f待つ
        yield return null;

        Debug.Log("wait select Start");
        //入力が決定されたら次に進む

        while (!Input.GetKeyDown("v")) {
            DialogSelect();
            yield return 0;
        }
        Debug.Log("wait select End");
        if (selectDialog)//「はい」を選択したときの処理
        {
            buyItem.PushBuyButton();

        }

        canGenerateDialog = true;
        messageTextObj.SetActive(false);

    }
    private void DialogSelect()
    {
        if (Input.GetKeyDown("a"))
        {
            selectDialog = true;
            DialogColorChange();
        }
        if (Input.GetKeyDown("d")) { 
            selectDialog = false;
            DialogColorChange();
        }

    }




    private void DialogColorChange()
    {
        if (selectDialog)//yesを選択
        {
            noText.color = gray;
            yesText.color = black;
        }
        else
        {
            noText.color = black;
            yesText.color = gray;
        }
        
    }
    private void TextInit()
    {
        selectDialog = true;
        noText.color = gray;
        yesText.color = black;
    }

    public void ChageModeToSell()
    {
        mode = ShopMode.sell;
    }
    public void ChageModeToUpdate()
    {
        mode = ShopMode.upgrade;
    }
    public void ChageModeToBuy()
    {
        mode = ShopMode.buy;
    }
   
    
}
