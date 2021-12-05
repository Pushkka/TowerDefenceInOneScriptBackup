using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class EptaScript : MonoBehaviour
{
    public ScriptType Type;
    public enum ScriptType
    {
        GlobalData,
        Mob,
        Tower,
        UnitTower,
        MainCamera,
        Unit,
        TowerPlace
    }

    public static Camera MainCam;
    public EptaScript SelectedTower;
    //GlobalData

    public Transform[] MobLinePoints;
    public int MainHealth;
    public int MainMoney;
    public int CurentVolna = -1;
    public GameObject[] MobVolna;
    public int LivedMobs = 0;
    public TextMeshProUGUI MoneyTextField;
    public TextMeshProUGUI WaveTextField;
    public TextMeshProUGUI HealthTextField;

    public static EptaScript GlobalData;
    private bool Delay;
    private float DelayTime = 9;

    //GlobalData END-----
    
    //Mob 

    public int MobHealth;
    public int MobCurentHealth;
    public int MobDamage;
    public int MobDamageToUnit;
    public float speed;
    public int MoneyForMob;
    public Transform Canvas;
    public Image HealthBar;
    private float MoveSpeed;
    private int CurentPoint = 0;
    public Transform MobTarget;
    private Vector3 GoToPos;
    private float MobAttackDelay;

    private float MobAndUnitMinDistance = 0.1f;

    private float HealthPercent;

    //Mob END-----

    //Tower

    public GameObject UpgradePressed;
    public GameObject DeletePressed;
    public bool Selected;
    public TextMeshProUGUI UpgradeCost;
    public GameObject TowerCanvas;

    //unit tower
    public List<EptaScript> Units = new List<EptaScript>();
    public Transform StartPointOnLine;
    public Transform StartPos1;
    public Transform StartPos2;
    public Transform StartPos3;
    private bool spawnUnit;

    public int CurentTowerLevel;
    public TowerLevel[] TowerLvl;

    [System.Serializable]
    public class TowerLevel
    {
        public float Range;
        public int Damage;
        public float Reload;
        public int UpgradeCoast;
        public int DestroyCoast;
        public GameObject Model;
        public GameObject Unit;
    }

    private float towerReloadTime;

    //Tower END-----

    //Unit

    public EptaScript MyUnitTower;
    public Transform UnitTarget;
    public Transform UnitStartPos;
    private float UnitAttackDelay; 
    public int UnitHealth;
    public int UnitCurentHealth;
    public int UnitDamage;
    public float UnitMoveSpeed;

    //Unit END------

    //TowerPlace

    public GameObject BuyTower1Pressed;
    public GameObject BuyTower2Pressed;
    public GameObject Tower1;
    public GameObject Tower2;
    public TextMeshProUGUI Tower1TextField;
    public TextMeshProUGUI Tower2TextField;

    //TowerPlace END-------

    void Start()
    {
        switch (Type)
        {
            case ScriptType.GlobalData:
                GlobalData = this;
                CurentVolna = -1;
                break;
            case ScriptType.Tower:
                UpgradeCost.text = TowerLvl[CurentTowerLevel + 1].UpgradeCoast.ToString();
                TowerLvl[CurentTowerLevel].Model.SetActive(true);
                break;
            case ScriptType.UnitTower:
                UpgradeCost.text = TowerLvl[CurentTowerLevel + 1].UpgradeCoast.ToString();
                TowerLvl[CurentTowerLevel].Model.SetActive(true);

                //���� ��������� ����� �� ����� �����
                float NearestPointDistance = 9999;
                Vector3 GlobalNearestPoint = new Vector3();
                for (int i = 0; i < GlobalData.MobLinePoints.Length -1; i++)
                {
                    Vector3 origin = GlobalData.MobLinePoints[i].position; 
                    Vector3 end = GlobalData.MobLinePoints[i + 1].position; 
                    Vector3 point = transform.position;
                    Vector3 CurentNearestPoint;
                    Vector3 heading = (end - origin);
                    float magnitudeMax = heading.magnitude;
                    heading.Normalize();

                    //Do projection from the point but clamp it
                    Vector3 lhs = point - origin;
                    float dotP = Vector3.Dot(lhs, heading);
                    dotP = Mathf.Clamp(dotP, 0f, magnitudeMax);
                    CurentNearestPoint = origin + heading * dotP;

                    if(Vector3.Distance(transform.position, CurentNearestPoint) < NearestPointDistance)
                    {
                        NearestPointDistance = Vector3.Distance(transform.position, CurentNearestPoint);
                        GlobalNearestPoint = CurentNearestPoint;
                    }

                    StartPointOnLine.position = GlobalNearestPoint;
                }
                break;
            case ScriptType.TowerPlace:
                Tower1TextField.text = Tower1.GetComponent<EptaScript>().TowerLvl[CurentTowerLevel].UpgradeCoast.ToString();
                Tower2TextField.text = Tower2.GetComponent<EptaScript>().TowerLvl[CurentTowerLevel].UpgradeCoast.ToString();
                break;
            case ScriptType.MainCamera:
                MainCam = GetComponent<Camera>();
                break;
        }
    }

    private void OnEnable()
    {
        switch (Type)
        {
            case ScriptType.Mob:
                MobCurentHealth = MobHealth;
                MoveSpeed = speed * 0.1f;
                GoToPos = EptaScript.GlobalData.MobLinePoints[0].position;
                EptaScript.GlobalData.LivedMobs++;
                break;

            case ScriptType.Unit:
                UnitCurentHealth = UnitHealth;
                UnitMoveSpeed = speed * 0.1f;
                break;
            case ScriptType.Tower:

                break;
        }
    }

    void Update()
    {
        switch (Type)
        {
            case ScriptType.GlobalData:
                MoneyTextField.text = MainMoney.ToString();
                WaveTextField.text = (CurentVolna +1).ToString();
                HealthTextField.text = MainHealth.ToString();
                if (LivedMobs <= 0 && !Delay)
                {
                    DelayTime = Time.time + 1;
                    Delay = true;
                }
                if(Delay && DelayTime < Time.time)
                {
                    CurentVolna++;
                    if (CurentVolna < MobVolna.Length)
                    {
                        MobVolna[CurentVolna].SetActive(true);
                        Delay = false;
                    }
                    else
                    {
                        Debug.Log("������ �����");
                    }
                }
                break;




            case ScriptType.Mob:
                Canvas.transform.rotation = Quaternion.Euler(40,-90,0);
                if (!MobTarget)
                {
                    transform.position = Vector3.MoveTowards(transform.position, GoToPos, MoveSpeed * Time.deltaTime);
                    transform.forward = Vector3.Lerp(transform.forward, GoToPos - transform.position, Time.deltaTime * 3);

                    if (Vector3.Distance(transform.position, GoToPos) < 0.1f)   //�������� �� ������
                    {
                        CurentPoint++;
                        if (CurentPoint < EptaScript.GlobalData.MobLinePoints.Length)
                        {
                            GoToPos = EptaScript.GlobalData.MobLinePoints[CurentPoint].position;
                        }
                        else
                        {
                            EptaScript.GlobalData.MainHealth -= MobDamage;
                            EptaScript.GlobalData.LivedMobs--;
                            Destroy(gameObject);
                        }
                    }
                    MobAttackDelay = 9999;
                }
                else
                {
                    if (Vector3.Distance(transform.position, MobTarget.position) < MobAndUnitMinDistance)   //�������� �� �����
                    {
                        if (MobAttackDelay == 9999)
                            MobAttackDelay = Time.time + 0.35f;

                        if (MobAttackDelay < Time.time)
                        {
                            MobTarget.GetComponent<EptaScript>().UnitCurentHealth -= MobDamageToUnit;
                            MobAttackDelay = Time.time + 0.75f;
                        }
                        transform.forward = Vector3.Lerp(transform.forward, MobTarget.position - transform.position, Time.deltaTime * 7);
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, MobTarget.position, MoveSpeed * Time.deltaTime);
                    }
                }

                //����� 

                HealthPercent = MobCurentHealth * 1f / MobHealth * 1f;
                HealthBar.fillAmount = HealthPercent;

                if (MobCurentHealth <= 0) //������
                {
                    EptaScript.GlobalData.LivedMobs--;
                    EptaScript.GlobalData.MainMoney += MoneyForMob;

                    Destroy(gameObject);
                }
                break;



            case ScriptType.Unit:
                Canvas.transform.rotation = Quaternion.Euler(40, -90, 0);
                if (!UnitTarget)
                {
                    if (Vector3.Distance(transform.position, UnitStartPos.position) > 0.05f)   //�������� �� ����� �����
                    {
                        transform.position = Vector3.MoveTowards(transform.position, UnitStartPos.position, UnitMoveSpeed * Time.deltaTime);
                        transform.forward = Vector3.Lerp(transform.forward, UnitStartPos.position - transform.position, Time.deltaTime * 3);
                    }
                    UnitAttackDelay = 9999;
                }
                else
                {
                    if (Vector3.Distance(transform.position, UnitTarget.position) < MobAndUnitMinDistance)   //�������� �� ����
                    {
                        if(UnitAttackDelay == 9999)
                        {
                            UnitAttackDelay = Time.time + 0.35f;
                        }
                        if (UnitAttackDelay < Time.time)
                        {
                            UnitTarget.GetComponent<EptaScript>().MobCurentHealth -= UnitDamage;
                            UnitAttackDelay = Time.time + 0.75f;
                        }
                        transform.forward = Vector3.Lerp(transform.forward, UnitTarget.position - transform.position, Time.deltaTime * 7);
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, UnitTarget.position, UnitMoveSpeed * Time.deltaTime);
                        transform.forward = Vector3.Lerp(transform.forward, UnitTarget.position - transform.position, Time.deltaTime * 3);
                    }
                }

                //����� �����

                HealthPercent = UnitCurentHealth * 1f / UnitHealth * 1f;
                HealthBar.fillAmount = HealthPercent;

                if (UnitCurentHealth <= 0) //������
                {
                    MyUnitTower.Units.Remove(this);
                    Destroy(gameObject);
                }
                break;



            case ScriptType.Tower:
                if (towerReloadTime < Time.time)
                {
                    Collider[] MobsInRange = Physics.OverlapSphere(transform.position, TowerLvl[CurentTowerLevel].Range, 1 << 6);
                    Debug.Log(MobsInRange.Length);
                    if (MobsInRange.Length > 0)
                    {
                        MobsInRange[MobsInRange.Length-1].GetComponent<EptaScript>().MobCurentHealth -= TowerLvl[CurentTowerLevel].Damage;
                        Debug.DrawLine(transform.position + Vector3.up*0.2f, MobsInRange[MobsInRange.Length - 1].transform.position, Color.cyan);
                        towerReloadTime = Time.time + TowerLvl[CurentTowerLevel].Reload;
                    }
                }
                if (UpgradePressed.activeSelf) //������ ������ ��������
                {
                    Debug.Log("�������");
                    if (CurentTowerLevel < TowerLvl.Length - 1)
                        if (GlobalData.MainMoney >= TowerLvl[CurentTowerLevel + 1].UpgradeCoast) // ������� ������� ������
                        {
                            TowerLvl[CurentTowerLevel].Model.SetActive(false);
                            GlobalData.MainMoney -= TowerLvl[CurentTowerLevel + 1].UpgradeCoast;
                            CurentTowerLevel++;
                            if (CurentTowerLevel < TowerLvl.Length - 1)
                                UpgradeCost.text = TowerLvl[CurentTowerLevel + 1].UpgradeCoast.ToString();
                            else
                                UpgradeCost.text = "Max";
                            TowerLvl[CurentTowerLevel].Model.SetActive(true);
                            TowerCanvas.SetActive(false);
                            UpgradePressed.SetActive(false);
                        }
                        else // ������� ������� �������
                        {
                            TowerCanvas.SetActive(false);
                            UpgradePressed.SetActive(false);
                        }
                }
                if (DeletePressed.activeSelf) //������ ������ ��������
                {
                    Debug.Log("���� �����");
                    GlobalData.MainMoney += TowerLvl[CurentTowerLevel].DestroyCoast;
                    Destroy(gameObject);
                }

                break;





            case ScriptType.UnitTower:
                if(Units.Count == 0 && !spawnUnit)
                {
                    towerReloadTime = Time.time + TowerLvl[CurentTowerLevel].Reload;
                    spawnUnit = true;
                }
                if (towerReloadTime < Time.time)
                {
                    if (spawnUnit)//���� ����� ������, ������� �����
                    {
                        Units.Add(Instantiate(TowerLvl[CurentTowerLevel].Unit, transform).GetComponent<EptaScript>());
                        Units.Add(Instantiate(TowerLvl[CurentTowerLevel].Unit, transform).GetComponent<EptaScript>());
                        Units.Add(Instantiate(TowerLvl[CurentTowerLevel].Unit, transform).GetComponent<EptaScript>());

                        Units[0].UnitStartPos = StartPos1;
                        Units[0].MyUnitTower = this;

                        Units[1].UnitStartPos = StartPos2;
                        Units[1].MyUnitTower = this;

                        Units[2].UnitStartPos = StartPos3;
                        Units[2].MyUnitTower = this;

                        spawnUnit = false;
                    }
                    Collider[] MobsInRange = Physics.OverlapSphere(transform.position, TowerLvl[CurentTowerLevel].Range, 1 << 6);

                    if (MobsInRange.Length == 1)
                    {
                        for (int i = 0; i < Units.Count; i++)
                            Units[i].UnitTarget = MobsInRange[0].transform;

                        MobsInRange[0].GetComponent<EptaScript>().MobTarget = Units[0].transform;
                    }
                    if (MobsInRange.Length == 2)
                    {
                        for (int i = 0; i < Units.Count; i++)
                            Units[i].UnitTarget = MobsInRange[0].transform;
                        for (int i = 1; i < Units.Count; i++)
                            Units[i].UnitTarget = MobsInRange[1].transform;

                        foreach (var item in MobsInRange)
                            item.GetComponent<EptaScript>().MobTarget = null;

                        for (int i = 0; i < Units.Count; i++)
                            MobsInRange[i].GetComponent<EptaScript>().MobTarget = Units[0].transform;
                    }
                    if (MobsInRange.Length >= 3)
                    {
                        for (int i = 0; i < Units.Count; i++) 
                            Units[i].UnitTarget = MobsInRange[0].transform;
                        for (int i = 1; i < Units.Count; i++)
                            Units[i].UnitTarget = MobsInRange[1].transform;
                        for (int i = 2; i < Units.Count; i++)
                            Units[i].UnitTarget = MobsInRange[2].transform;

                        foreach (var item in MobsInRange)
                            item.GetComponent<EptaScript>().MobTarget = null;

                        for (int i = 0; i < Units.Count; i++)
                            MobsInRange[i].GetComponent<EptaScript>().MobTarget = Units[0].transform;
                    }
                }
                if (UpgradePressed.activeSelf) //������ ������ ��������
                {
                    Debug.Log("�������");
                    if (CurentTowerLevel < TowerLvl.Length - 1)
                        if (GlobalData.MainMoney >= TowerLvl[CurentTowerLevel + 1].UpgradeCoast) // ������� ������� ������
                        {
                            TowerLvl[CurentTowerLevel].Model.SetActive(false);
                            GlobalData.MainMoney -= TowerLvl[CurentTowerLevel + 1].UpgradeCoast;
                            CurentTowerLevel++;
                            if (CurentTowerLevel < TowerLvl.Length - 1)
                                UpgradeCost.text = TowerLvl[CurentTowerLevel + 1].UpgradeCoast.ToString();
                            else
                                UpgradeCost.text = "Max";
                            TowerLvl[CurentTowerLevel].Model.SetActive(true);
                            TowerCanvas.SetActive(false);
                            UpgradePressed.SetActive(false);
                        }
                        else // ������� ������� �������
                        {
                            TowerCanvas.SetActive(false);
                            UpgradePressed.SetActive(false);
                        }
                }
                if (DeletePressed.activeSelf) //������ ������ ��������
                {
                    Debug.Log("���� �����");
                    GlobalData.MainMoney += TowerLvl[CurentTowerLevel].DestroyCoast;
                    Destroy(gameObject);
                }
                break;




            case ScriptType.TowerPlace:
                if (BuyTower1Pressed.activeSelf) //������ ������ ��������
                {
                    Debug.Log("�������� ����� 1");
                    if (GlobalData.MainMoney >= Tower1.GetComponent<EptaScript>().TowerLvl[CurentTowerLevel].UpgradeCoast) // ������� ������� ������
                    {
                        GlobalData.MainMoney -= Tower1.GetComponent<EptaScript>().TowerLvl[CurentTowerLevel].UpgradeCoast;

                        Instantiate(Tower1, transform.position, Quaternion.identity);

                        TowerCanvas.SetActive(false);
                        BuyTower1Pressed.SetActive(false);
                    }
                    else // ������� ������� �������
                    {
                        TowerCanvas.SetActive(false);
                        BuyTower1Pressed.SetActive(false);
                    }
                }
                if (BuyTower2Pressed.activeSelf) //������ ������ ��������
                {
                    Debug.Log("�������� ����� 2");
                    if (GlobalData.MainMoney >= Tower2.GetComponent<EptaScript>().TowerLvl[CurentTowerLevel].UpgradeCoast) // ������� ������� ������
                    {
                        GlobalData.MainMoney -= Tower2.GetComponent<EptaScript>().TowerLvl[CurentTowerLevel].UpgradeCoast;

                        Instantiate(Tower2, transform.position, Quaternion.identity);

                        TowerCanvas.SetActive(false);
                        BuyTower2Pressed.SetActive(false);
                    }
                    else // ������� ������� �������
                    {
                        TowerCanvas.SetActive(false);
                        BuyTower2Pressed.SetActive(false);
                    }
                }
                break;



            case ScriptType.MainCamera:
                if (Input.GetMouseButtonUp(0))
                {
                    RaycastHit hit;
                    Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 100 ,1 << 7))
                    {
                        Transform objectHit = hit.transform;
                        Debug.Log(objectHit.name);
                        EptaScript Epta = objectHit.GetComponent<EptaScript>();
                        if (Epta.Type == ScriptType.Tower || Epta.Type == ScriptType.UnitTower || Epta.Type == ScriptType.TowerPlace)
                        {
                            if(SelectedTower != null)
                            {
                                SelectedTower.TowerCanvas.SetActive(false);
                            }
                            SelectedTower = Epta;
                            SelectedTower.TowerCanvas.SetActive(true);
                        }
                    }
                    else
                    {
                        if(SelectedTower != null)
                        {
                            SelectedTower.TowerCanvas.SetActive(false);
                            SelectedTower = null;
                        }
                    }
                }
                break;
        }
    }

    private void OnDrawGizmos()
    {
        switch (Type)
        {
            case ScriptType.GlobalData:
                Gizmos.color = Color.red;
                if(MobLinePoints.Length > 2)
                for (int i = 1; i < MobLinePoints.Length; i++)
                {
                    Gizmos.DrawLine(MobLinePoints[i].position, MobLinePoints[i - 1].position);
                }
                break;
            case ScriptType.Mob:

                break;
            case ScriptType.Tower:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, TowerLvl[CurentTowerLevel].Range);
                break;
            case ScriptType.UnitTower:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, TowerLvl[CurentTowerLevel].Range);
                break;
            default:
                break;
        }
    }
}

[CustomEditor(typeof(EptaScript))]
[CanEditMultipleObjects]
public class EptaScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty Type = serializedObject.FindProperty("Type");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Type"), new GUIContent("��� �������"));

        switch (Type.enumValueIndex)
        {
            case 0:
                if (!Type.hasMultipleDifferentValues)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("MobLinePoints"), new GUIContent("������� ������"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("MainHealth"), new GUIContent("�����"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("MainMoney"), new GUIContent("������"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("MobVolna"), new GUIContent("������ ����"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("LivedMobs"), new GUIContent("�������� ����� �����"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("MoneyTextField"), new GUIContent("����� �����"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("WaveTextField"), new GUIContent("����� �����"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("HealthTextField"), new GUIContent("����� ������"));
                }
                break;
            case 1:
                if (!Type.hasMultipleDifferentValues)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("MobHealth"), new GUIContent("������������ ����� �����"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("MobCurentHealth"), new GUIContent("�����"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("MobDamage"), new GUIContent("���� �����"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("MobDamageToUnit"), new GUIContent("���� �����"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"), new GUIContent("��������"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("MoneyForMob"), new GUIContent("����� �� ��������"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("Canvas"), new GUIContent("������"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("HealthBar"), new GUIContent("��� ��"));
                }
                break;
            case 2:
                if (!Type.hasMultipleDifferentValues)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("CurentTowerLevel"), new GUIContent("������� �������"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("TowerLvl"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("UpgradePressed"), new GUIContent("������� ��� ��������"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("DeletePressed"), new GUIContent("������� ��� �����"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("TowerCanvas"), new GUIContent("������ ������"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("UpgradeCost"), new GUIContent("����� ����"));

                }
                break;

            case 3:
                if (!Type.hasMultipleDifferentValues)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("CurentTowerLevel"), new GUIContent("������� �������"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("TowerLvl"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("Units"), new GUIContent("�����"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("StartPointOnLine"), new GUIContent("������� ����� �� �����"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("StartPos1"), new GUIContent("������� ����� 1"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("StartPos2"), new GUIContent("������� ����� 2"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("StartPos3"), new GUIContent("������� ����� 3"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("UpgradePressed"), new GUIContent("������� ��� ��������"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("DeletePressed"), new GUIContent("������� ��� �����"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("TowerCanvas"), new GUIContent("������ ������"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("UpgradeCost"), new GUIContent("����� ����"));

                }

                break;

            case 4:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("SelectedTower"), new GUIContent("��������� �����"));

                break;

            case 5:

                EditorGUILayout.PropertyField(serializedObject.FindProperty("UnitHealth"), new GUIContent("������������ ����� �����"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("UnitCurentHealth"), new GUIContent("�����"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("UnitDamage"), new GUIContent("���� �������"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"), new GUIContent("��������"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Canvas"), new GUIContent("������"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("HealthBar"), new GUIContent("��� ��"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("MyUnitTower"), new GUIContent("����"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("UnitStartPos"), new GUIContent("��������� �������"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("UnitTarget"), new GUIContent("����"));
                break;

            case 6:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TowerCanvas"), new GUIContent("������ ������"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("BuyTower1Pressed"), new GUIContent("������� 1"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("BuyTower2Pressed"), new GUIContent("������� 2"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Tower1"), new GUIContent("���� ����� 1"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Tower2"), new GUIContent("���� ����� 2"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Tower1TextField"), new GUIContent("����� ���� ����� 1"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Tower2TextField"), new GUIContent("����� ���� ����� 2"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
