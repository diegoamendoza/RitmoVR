using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
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

    private InputDevice leftHandDevice;
    private InputDevice rightHandDevice;

    void Start()
    {
        m_LTriggerInput?.EnableDirectActionIfModeUsed();
        m_RTriggerInput?.EnableDirectActionIfModeUsed();
        m_LGripInput?.EnableDirectActionIfModeUsed();
        m_RGripInput?.EnableDirectActionIfModeUsed();

        // Encuentra los dispositivos para las manos izquierda y derecha
        leftHandDevice = GetDevice(XRNode.LeftHand);
        rightHandDevice = GetDevice(XRNode.RightHand);
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
                m_LRenderer.gameObject.tag = "Untagged";
                break;
            case 0.5f:
                m_LRenderer.material = m_LMaterial[1];
                m_LRenderer.gameObject.tag = "LRed";
                break;
            case 1:
                m_LRenderer.material = m_LMaterial[2];
                m_LRenderer.gameObject.tag = "LBlue";
                break;
            case 1.5f:
                m_LRenderer.material = m_LMaterial[3];
                m_LRenderer.gameObject.tag = "LPurple";
                break;
        }

        switch (RValue)
        {
            case 0:
                m_RRenderer.material = m_RMaterial[0];
                m_RRenderer.gameObject.tag = "Untagged";
                break;
            case 0.5f:
                m_RRenderer.material = m_RMaterial[1];
                m_RRenderer.gameObject.tag = "RRed";
                break;
            case 1:
                m_RRenderer.material = m_RMaterial[2];
                m_RRenderer.gameObject.tag = "RBlue";
                break;
            case 1.5f:
                m_RRenderer.material = m_RMaterial[3];
                m_RRenderer.gameObject.tag = "RPurple";
                break;
        }
    }

    
    private InputDevice GetDevice(XRNode node)
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(node, devices);
        return devices.Count > 0 ? devices[0] : default;
    }

    
    public void VibrateController(bool isLeftHand, float intensity, float duration)
    {
        InputDevice device = isLeftHand ? leftHandDevice : rightHandDevice;

        if (device.isValid)
        {
            device.SendHapticImpulse(0, intensity, duration);
        }
        else
        {
            Debug.LogWarning("No se encontró el controlador especificado.");
        }
    }
}
