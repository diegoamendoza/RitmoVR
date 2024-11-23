using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

public class ControllerReader : MonoBehaviour
{
    [SerializeField]
    XRInputValueReader<float> m_LTriggerInput = new XRInputValueReader<float>("Trigger");
    float LtriggerVal;
    [SerializeField]
    XRInputValueReader<float> m_LGripInput = new XRInputValueReader<float>("Grip");
    float LgripVal;
    [SerializeField]
    XRInputValueReader<float> m_RTriggerInput = new XRInputValueReader<float>("Trigger");
    float RtriggerVal;
    [SerializeField]
    XRInputValueReader<float> m_RGripInput = new XRInputValueReader<float>("Grip");
    float RgripVal;

    [SerializeField]
    MeshRenderer m_LRenderer;
    [SerializeField]
    MeshRenderer m_RRenderer;
    [SerializeField]
    Material[] m_LMaterial;
    [SerializeField]
    Material[] m_RMaterial;

    [SerializeField]
    float LValue;
    [SerializeField]
    float RValue;

    void Start()
    {
        m_LTriggerInput?.EnableDirectActionIfModeUsed();
        m_RTriggerInput?.EnableDirectActionIfModeUsed();
        m_LGripInput?.EnableDirectActionIfModeUsed();
        m_RGripInput?.EnableDirectActionIfModeUsed();

    }


    void Update()
    {
        LtriggerVal = m_LTriggerInput.ReadValue();
        LgripVal = m_LGripInput.ReadValue();
        RgripVal = m_RGripInput.ReadValue();
        RtriggerVal = m_RTriggerInput.ReadValue();

        LValue = LtriggerVal + (LgripVal * 0.5f);
        RValue = RtriggerVal + (RgripVal * 0.5f);

        switch (LValue)
        {
            case 0:
                m_LRenderer.material = m_LMaterial[0];
                break;
            case 0.5f:
                m_LRenderer.material = m_LMaterial[1];
                break;
            case 1:
                m_LRenderer.material = m_LMaterial[2];
                break;
            case 1.5f:
                m_LRenderer.material = m_LMaterial[3];
                break;
        }

        switch (RValue)
        {
            case 0:
                m_RRenderer.material = m_RMaterial[0];
                break;
            case 0.5f:
                m_RRenderer.material = m_RMaterial[1];
                break;
            case 1:
                m_RRenderer.material = m_RMaterial[2];
                break;
            case 1.5f:
                m_RRenderer.material = m_RMaterial[3];
                break;

        }


        //Debug.Log("Left trigger: " + LtriggerVal);
        //Debug.Log("Right trigger: " + RtriggerVal);
        //Debug.Log("Left Grip: " + LgripVal);
        //Debug.Log("Right Grip: " + RgripVal);

    }
}
