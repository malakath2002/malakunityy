using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MedicineInteract : MonoBehaviour
{
    [Header("Shelf")]
    public string shelfName;

    [Header("Medicines")]
    public string[] medicineNames;
    public string[] descriptions;
    public string[] dosages;
    public string[] warnings;

    [Header("UI")]
    public GameObject medicineButtonPrefab;
    public Transform buttonContainer;

    static GameObject panel;

    static TextMeshProUGUI titleText;
    static TextMeshProUGUI infoText;

    static bool uiSetup = false;

    void Start()
    {
        if (!uiSetup)
        {
            SetupUI();
            uiSetup = true;
        }
    }

    void Update()
    {
        // إذا ضغط على UI لا يعمل Raycast على العالم
        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray =
                Camera.main.ScreenPointToRay(
                    Input.mousePosition
                );

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                MedicineInteract med =
                    hit.collider.GetComponentInParent<MedicineInteract>();

                if (med != null)
                {
                    med.OpenShelf();
                }
            }
        }
    }

    // تجهيز الـ UI
    static void SetupUI()
    {
        panel = GameObject.Find("MedicinePanel");

        if (panel == null)
        {
            Debug.LogError("MedicinePanel not found!");
            return;
        }

        titleText =
            panel.transform.Find("Text (TMP)")
            .GetComponent<TextMeshProUGUI>();

        GameObject infoObj =
            GameObject.Find("Info");

        if (infoObj == null)
        {
            Debug.LogError("Info object not found!");
            return;
        }

        infoText =
            infoObj.GetComponent<TextMeshProUGUI>();

        panel.SetActive(false);
    }

    // فتح الرف
    public void OpenShelf()
{
    Debug.Log("OPEN SHELF WORKED");

    panel.SetActive(true);


    titleText.text =
        shelfName.Replace("_", " ");

    infoText.text =
        "Choose a medicine";

    // حذف الأزرار القديمة
    foreach (Transform child in buttonContainer)
    {
        Destroy(child.gameObject);
    }

    // إنشاء الأزرار
    for (int i = 0; i < medicineNames.Length; i++)
    {
        int index = i;

        GameObject btnObj =
            Instantiate(
                medicineButtonPrefab,
                buttonContainer
            );

        Debug.Log("CREATED BUTTON -> " + medicineNames[index]);

        btnObj.SetActive(true);

        // التأكد من أن الزر ظاهر
        btnObj.transform.SetAsLastSibling();

        // النص
        TextMeshProUGUI txt =
            btnObj.GetComponentInChildren<TextMeshProUGUI>();

        if (txt != null)
        {
            txt.text = medicineNames[index];
            txt.raycastTarget = false;
        }

        // الزر
        Button btn =
            btnObj.GetComponent<Button>();

        if (btn != null)
        {
            Debug.Log("BUTTON COMPONENT FOUND");

            btn.onClick.RemoveAllListeners();

            btn.onClick.AddListener(() =>
            {
                Debug.Log("BUTTON WORKED -> " + medicineNames[index]);
                ShowMedicine(index);
            });
        }
        else
        {
            Debug.LogError(
                "Button component not found in MedicineButtonPrefab!"
            );
        }
    }
}

    // عرض معلومات الدواء
 void ShowMedicine(int index)
{
    Debug.Log("PRESSED -> " + medicineNames[index]);

    infoText.text =
        "<b>" + medicineNames[index] + "</b>\n\n" +
        descriptions[index] +
        "\n\nDosage: " + dosages[index] +
        "\n\nWarning: " + warnings[index];
}

    // إغلاق الـ UI
    public void CloseUI()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}