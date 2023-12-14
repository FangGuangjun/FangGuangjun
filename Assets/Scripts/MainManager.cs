using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    private Animator m_BetalRotate_1_Ani;
    private Animator m_HandTipsAni;
    private Animator m_BetalRotate_2_Ani;
    private Image m_Process_bar_prompt_2;
    public Button m_StartRotateBtn;
    public GameObject Egg;
    public Texture[] Egg_Texture;
    private Material Egg_Material;
    private bool IsOne;
    private bool IsOne1;
    private bool IsOne2;
    private float timer;
    public ParticleSystem eggEff;
    public ParticleSystem endEff;
    private float conut;
    private Vector3 oldPosition;
    void Start()
    {
        oldPosition=transform.position;
        //所有动画初始化
        m_BetalRotate_1_Ani = transform.Find("betals/betal").GetComponent<Animator>();
        m_BetalRotate_1_Ani.speed = 0f;
        m_BetalRotate_2_Ani = transform.Find("betals/betal_1").GetComponent<Animator>();
        m_BetalRotate_2_Ani.speed = 0f;
        m_HandTipsAni = transform.Find("click prompt&drag prompt").GetComponent<Animator>();
        m_HandTipsAni.speed = 1f;
        m_Process_bar_prompt_2 = transform.Find("Canvas/Process bar prompt_2").GetComponent<Image>();
        m_StartRotateBtn.onClick.AddListener(() =>
        {
            m_Process_bar_prompt_2.transform.parent.gameObject.SetActive(true);
            m_BetalRotate_1_Ani.speed = 2f;
            m_BetalRotate_2_Ani.speed = 2f;
            m_StartRotateBtn.interactable = false;
            StartCoroutine(Progressbar());
            eggEff.Play();
        });
        MeshRenderer meshRenderer = Egg.GetComponent<MeshRenderer>();
        Egg_Material = meshRenderer.material;
    }

    //进度条协程
    IEnumerator Progressbar()
    {
        while (true)
        {
            timer += Time.deltaTime;
            conut=timer / 5f;
            m_Process_bar_prompt_2.fillAmount = timer / 5f;
            if (conut >= 0.5f && conut < 0.75f && !IsOne)
            {
                IsOne = true;
                Egg_Material.mainTexture = Egg_Texture[0];
            }
            else if (conut >= 0.75f && conut < 1f && !IsOne1)
            {
                Egg_Material.mainTexture = Egg_Texture[1];
                IsOne1 = true;
            }
            else if (conut >= 1f && conut < 1.1f && !IsOne2)
            {
                Egg_Material.mainTexture = Egg_Texture[2];
                IsOne2 = true;
            }
            //进度条满，动画结束，特效开关，进度条隐藏
            else if (conut >= 1.1f)
            {
                m_BetalRotate_1_Ani.speed = 0f;
                m_BetalRotate_2_Ani.speed = 0f;
                eggEff.Stop();
                endEff.Play();
                m_Process_bar_prompt_2.transform.parent.gameObject.SetActive(false);
                transform.position = oldPosition;
                break;
            }
            yield return null;
        }
    }

    // Update is called once per frame

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "egg")
        {
            m_StartRotateBtn.gameObject.SetActive(true);
        }
    }


    private bool isDragging = false; // 是否正在拖拽
    private Vector3 offset; // 鼠标点击点与物体原始位置的偏移量


    private void OnMouseDown()
    {
        // 计算鼠标点击点与物体原始位置的偏移量
        offset = transform.position - GetMouseWorldPosition();
        m_HandTipsAni.speed = 0f;
        m_HandTipsAni.gameObject.SetActive(false);
        // 开始拖拽
        isDragging = true;
    }

    private void OnMouseUp()
    {
        // 停止拖拽
        isDragging = false;
    }

    private void Update()
    {
        if (isDragging)
        {
            // 更新物体位置为当前鼠标位置加上偏移量
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        // 将鼠标当前屏幕坐标转换为世界坐标
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
