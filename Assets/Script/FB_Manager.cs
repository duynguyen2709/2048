using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FB_Manager : MonoBehaviour
{

    public GameObject LoggedInManager;
    public GameObject LoggedOutManager;
    public GameObject UsernameManager;
    public GameObject AvatarManager;

    void Awake()
    {
        FB.Init(SetInit,OnHideUnity);
    }

    private void SetInit()
    {
        if (FB.IsInitialized)
        {
            Debug.Log("Init done");
        }
        else
        {
            Debug.Log("Init failed");
        }
        MenuController(FB.IsLoggedIn);
    }

    private void OnHideUnity(bool isunityshown)
    {
        if (!isunityshown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void FB_Login()
    {
        List<string> permissions = new List<string>();
        permissions.Add("public_profile");
        permissions.Add("user_friends");
        FB.LogInWithReadPermissions(permissions,AuthCallBack);
    }

    private void AuthCallBack(IResult result)
    {
        if (result.Error != null)
        {
            Debug.Log(result.Error);
        }
        else
        {
            if (FB.IsLoggedIn)
            {
                Debug.Log("FB is logged in");
            }
            else
            {
                Debug.Log("FB is not logged in");
            }
            MenuController(FB.IsLoggedIn);
        }
    }

    private void MenuController(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            LoggedInManager.SetActive(true);
            LoggedOutManager.SetActive(false);
            FB.API("/me?fields=name",HttpMethod.GET,DisplayUsername);
            FB.API("/me/picture?width=128&height=128", HttpMethod.GET,DisplayAvatar);
        }
        else
        {
            LoggedInManager.SetActive(false);
            LoggedOutManager.SetActive(true);
        }
    }

    private void DisplayAvatar(IGraphResult result)
    {
        if (result.Error != null)
        {
            Texture2D avatar = result.Texture;
            AvatarManager.GetComponent<Image>().sprite = Sprite.Create(avatar, new Rect(0, 0, 100, 100), new Vector2(0.5f, 0.5f));
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    private void DisplayUsername(IResult result)
    {
        Text username = UsernameManager.GetComponent<Text>();
        if (result.Error == null)
        {
            username.text = result.ResultDictionary["name"].ToString();
        }
        else
        {
            Debug.Log(result.Error);
        }
    }
}
