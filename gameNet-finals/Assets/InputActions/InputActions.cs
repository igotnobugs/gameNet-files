// GENERATED AUTOMATICALLY FROM 'Assets/InputActions/InputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""Hero Move Control"",
            ""id"": ""974368e7-f2bc-4ede-ae9c-e164a0a68f28"",
            ""actions"": [
                {
                    ""name"": ""MoveClick"",
                    ""type"": ""Button"",
                    ""id"": ""c629e001-c495-4329-921c-f25dc5cf074b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MovePosition"",
                    ""type"": ""Value"",
                    ""id"": ""c11fcf33-859c-40fe-a9fc-8dac9be7043e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""c296d947-a508-4204-8077-fc4cafcc69ac"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AttackClick"",
                    ""type"": ""Button"",
                    ""id"": ""e2c2da1b-cf04-44c4-b3c3-2a267112f9da"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7866c17b-a2f1-4076-80f0-40c3a2ba7fe3"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""MoveClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""970fedac-6f60-4ffd-a492-8c8b92fc0f7b"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""MovePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6f4d1b72-a13a-4159-9c26-369720901d37"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""42efe3d9-8a30-4f8c-b0f1-75e74466c72d"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""AttackClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Camera Control"",
            ""id"": ""2d7e8112-e020-446e-b085-e98b379d1b38"",
            ""actions"": [
                {
                    ""name"": ""WASD"",
                    ""type"": ""Value"",
                    ""id"": ""2724a6a6-f0c9-4715-b3b7-d85600b6a711"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TrackPlayerHero"",
                    ""type"": ""Button"",
                    ""id"": ""d89ff36a-68ce-40de-9598-e0c4dd963f8b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""d98a7565-d28c-4058-b6cd-5d21baee5d60"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASD"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b6b7f88b-ccbb-41ae-8094-9049eaca8d27"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""WASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""04aafe01-8b43-4a4b-b0c2-9aeb1d38b642"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""WASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6cd49465-c195-431d-b5d8-64d1c4898ea0"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""WASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ce4040d5-99e6-4a0a-bf0b-23fe42fc4bdc"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""WASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""0290463e-4547-4039-b033-6c9bb41e7892"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""TrackPlayerHero"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Hero Skills Control"",
            ""id"": ""ef7d0aae-7244-4ec1-890b-e6f83e3db0ac"",
            ""actions"": [
                {
                    ""name"": ""Skill 1"",
                    ""type"": ""Button"",
                    ""id"": ""e1d2a63d-92c8-4a5d-afb5-c3001fd0f222"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Skill 2"",
                    ""type"": ""Button"",
                    ""id"": ""0984182d-2168-4f2b-89d4-a275a4bd829e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Skill 3"",
                    ""type"": ""Button"",
                    ""id"": ""bfa71e4b-db65-405e-8e59-da56884000fa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Skill 4"",
                    ""type"": ""Button"",
                    ""id"": ""ea739b31-8dda-4125-b73f-f3ea7d62abed"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""13842fb0-5f1c-4634-a782-9237d0f2a4c2"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Skill 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""11dbe21d-bad6-4d22-80c1-9033c0874971"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Skill 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""646c4bf4-8f14-46f4-ad81-a2896a788563"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Skill 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f70f9af8-bc97-4c86-83df-42e7ceeb7fe4"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Skill 4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Default"",
            ""bindingGroup"": ""Default"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Hero Move Control
        m_HeroMoveControl = asset.FindActionMap("Hero Move Control", throwIfNotFound: true);
        m_HeroMoveControl_MoveClick = m_HeroMoveControl.FindAction("MoveClick", throwIfNotFound: true);
        m_HeroMoveControl_MovePosition = m_HeroMoveControl.FindAction("MovePosition", throwIfNotFound: true);
        m_HeroMoveControl_Attack = m_HeroMoveControl.FindAction("Attack", throwIfNotFound: true);
        m_HeroMoveControl_AttackClick = m_HeroMoveControl.FindAction("AttackClick", throwIfNotFound: true);
        // Camera Control
        m_CameraControl = asset.FindActionMap("Camera Control", throwIfNotFound: true);
        m_CameraControl_WASD = m_CameraControl.FindAction("WASD", throwIfNotFound: true);
        m_CameraControl_TrackPlayerHero = m_CameraControl.FindAction("TrackPlayerHero", throwIfNotFound: true);
        // Hero Skills Control
        m_HeroSkillsControl = asset.FindActionMap("Hero Skills Control", throwIfNotFound: true);
        m_HeroSkillsControl_Skill1 = m_HeroSkillsControl.FindAction("Skill 1", throwIfNotFound: true);
        m_HeroSkillsControl_Skill2 = m_HeroSkillsControl.FindAction("Skill 2", throwIfNotFound: true);
        m_HeroSkillsControl_Skill3 = m_HeroSkillsControl.FindAction("Skill 3", throwIfNotFound: true);
        m_HeroSkillsControl_Skill4 = m_HeroSkillsControl.FindAction("Skill 4", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Hero Move Control
    private readonly InputActionMap m_HeroMoveControl;
    private IHeroMoveControlActions m_HeroMoveControlActionsCallbackInterface;
    private readonly InputAction m_HeroMoveControl_MoveClick;
    private readonly InputAction m_HeroMoveControl_MovePosition;
    private readonly InputAction m_HeroMoveControl_Attack;
    private readonly InputAction m_HeroMoveControl_AttackClick;
    public struct HeroMoveControlActions
    {
        private @InputActions m_Wrapper;
        public HeroMoveControlActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveClick => m_Wrapper.m_HeroMoveControl_MoveClick;
        public InputAction @MovePosition => m_Wrapper.m_HeroMoveControl_MovePosition;
        public InputAction @Attack => m_Wrapper.m_HeroMoveControl_Attack;
        public InputAction @AttackClick => m_Wrapper.m_HeroMoveControl_AttackClick;
        public InputActionMap Get() { return m_Wrapper.m_HeroMoveControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(HeroMoveControlActions set) { return set.Get(); }
        public void SetCallbacks(IHeroMoveControlActions instance)
        {
            if (m_Wrapper.m_HeroMoveControlActionsCallbackInterface != null)
            {
                @MoveClick.started -= m_Wrapper.m_HeroMoveControlActionsCallbackInterface.OnMoveClick;
                @MoveClick.performed -= m_Wrapper.m_HeroMoveControlActionsCallbackInterface.OnMoveClick;
                @MoveClick.canceled -= m_Wrapper.m_HeroMoveControlActionsCallbackInterface.OnMoveClick;
                @MovePosition.started -= m_Wrapper.m_HeroMoveControlActionsCallbackInterface.OnMovePosition;
                @MovePosition.performed -= m_Wrapper.m_HeroMoveControlActionsCallbackInterface.OnMovePosition;
                @MovePosition.canceled -= m_Wrapper.m_HeroMoveControlActionsCallbackInterface.OnMovePosition;
                @Attack.started -= m_Wrapper.m_HeroMoveControlActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_HeroMoveControlActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_HeroMoveControlActionsCallbackInterface.OnAttack;
                @AttackClick.started -= m_Wrapper.m_HeroMoveControlActionsCallbackInterface.OnAttackClick;
                @AttackClick.performed -= m_Wrapper.m_HeroMoveControlActionsCallbackInterface.OnAttackClick;
                @AttackClick.canceled -= m_Wrapper.m_HeroMoveControlActionsCallbackInterface.OnAttackClick;
            }
            m_Wrapper.m_HeroMoveControlActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveClick.started += instance.OnMoveClick;
                @MoveClick.performed += instance.OnMoveClick;
                @MoveClick.canceled += instance.OnMoveClick;
                @MovePosition.started += instance.OnMovePosition;
                @MovePosition.performed += instance.OnMovePosition;
                @MovePosition.canceled += instance.OnMovePosition;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @AttackClick.started += instance.OnAttackClick;
                @AttackClick.performed += instance.OnAttackClick;
                @AttackClick.canceled += instance.OnAttackClick;
            }
        }
    }
    public HeroMoveControlActions @HeroMoveControl => new HeroMoveControlActions(this);

    // Camera Control
    private readonly InputActionMap m_CameraControl;
    private ICameraControlActions m_CameraControlActionsCallbackInterface;
    private readonly InputAction m_CameraControl_WASD;
    private readonly InputAction m_CameraControl_TrackPlayerHero;
    public struct CameraControlActions
    {
        private @InputActions m_Wrapper;
        public CameraControlActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @WASD => m_Wrapper.m_CameraControl_WASD;
        public InputAction @TrackPlayerHero => m_Wrapper.m_CameraControl_TrackPlayerHero;
        public InputActionMap Get() { return m_Wrapper.m_CameraControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraControlActions set) { return set.Get(); }
        public void SetCallbacks(ICameraControlActions instance)
        {
            if (m_Wrapper.m_CameraControlActionsCallbackInterface != null)
            {
                @WASD.started -= m_Wrapper.m_CameraControlActionsCallbackInterface.OnWASD;
                @WASD.performed -= m_Wrapper.m_CameraControlActionsCallbackInterface.OnWASD;
                @WASD.canceled -= m_Wrapper.m_CameraControlActionsCallbackInterface.OnWASD;
                @TrackPlayerHero.started -= m_Wrapper.m_CameraControlActionsCallbackInterface.OnTrackPlayerHero;
                @TrackPlayerHero.performed -= m_Wrapper.m_CameraControlActionsCallbackInterface.OnTrackPlayerHero;
                @TrackPlayerHero.canceled -= m_Wrapper.m_CameraControlActionsCallbackInterface.OnTrackPlayerHero;
            }
            m_Wrapper.m_CameraControlActionsCallbackInterface = instance;
            if (instance != null)
            {
                @WASD.started += instance.OnWASD;
                @WASD.performed += instance.OnWASD;
                @WASD.canceled += instance.OnWASD;
                @TrackPlayerHero.started += instance.OnTrackPlayerHero;
                @TrackPlayerHero.performed += instance.OnTrackPlayerHero;
                @TrackPlayerHero.canceled += instance.OnTrackPlayerHero;
            }
        }
    }
    public CameraControlActions @CameraControl => new CameraControlActions(this);

    // Hero Skills Control
    private readonly InputActionMap m_HeroSkillsControl;
    private IHeroSkillsControlActions m_HeroSkillsControlActionsCallbackInterface;
    private readonly InputAction m_HeroSkillsControl_Skill1;
    private readonly InputAction m_HeroSkillsControl_Skill2;
    private readonly InputAction m_HeroSkillsControl_Skill3;
    private readonly InputAction m_HeroSkillsControl_Skill4;
    public struct HeroSkillsControlActions
    {
        private @InputActions m_Wrapper;
        public HeroSkillsControlActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Skill1 => m_Wrapper.m_HeroSkillsControl_Skill1;
        public InputAction @Skill2 => m_Wrapper.m_HeroSkillsControl_Skill2;
        public InputAction @Skill3 => m_Wrapper.m_HeroSkillsControl_Skill3;
        public InputAction @Skill4 => m_Wrapper.m_HeroSkillsControl_Skill4;
        public InputActionMap Get() { return m_Wrapper.m_HeroSkillsControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(HeroSkillsControlActions set) { return set.Get(); }
        public void SetCallbacks(IHeroSkillsControlActions instance)
        {
            if (m_Wrapper.m_HeroSkillsControlActionsCallbackInterface != null)
            {
                @Skill1.started -= m_Wrapper.m_HeroSkillsControlActionsCallbackInterface.OnSkill1;
                @Skill1.performed -= m_Wrapper.m_HeroSkillsControlActionsCallbackInterface.OnSkill1;
                @Skill1.canceled -= m_Wrapper.m_HeroSkillsControlActionsCallbackInterface.OnSkill1;
                @Skill2.started -= m_Wrapper.m_HeroSkillsControlActionsCallbackInterface.OnSkill2;
                @Skill2.performed -= m_Wrapper.m_HeroSkillsControlActionsCallbackInterface.OnSkill2;
                @Skill2.canceled -= m_Wrapper.m_HeroSkillsControlActionsCallbackInterface.OnSkill2;
                @Skill3.started -= m_Wrapper.m_HeroSkillsControlActionsCallbackInterface.OnSkill3;
                @Skill3.performed -= m_Wrapper.m_HeroSkillsControlActionsCallbackInterface.OnSkill3;
                @Skill3.canceled -= m_Wrapper.m_HeroSkillsControlActionsCallbackInterface.OnSkill3;
                @Skill4.started -= m_Wrapper.m_HeroSkillsControlActionsCallbackInterface.OnSkill4;
                @Skill4.performed -= m_Wrapper.m_HeroSkillsControlActionsCallbackInterface.OnSkill4;
                @Skill4.canceled -= m_Wrapper.m_HeroSkillsControlActionsCallbackInterface.OnSkill4;
            }
            m_Wrapper.m_HeroSkillsControlActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Skill1.started += instance.OnSkill1;
                @Skill1.performed += instance.OnSkill1;
                @Skill1.canceled += instance.OnSkill1;
                @Skill2.started += instance.OnSkill2;
                @Skill2.performed += instance.OnSkill2;
                @Skill2.canceled += instance.OnSkill2;
                @Skill3.started += instance.OnSkill3;
                @Skill3.performed += instance.OnSkill3;
                @Skill3.canceled += instance.OnSkill3;
                @Skill4.started += instance.OnSkill4;
                @Skill4.performed += instance.OnSkill4;
                @Skill4.canceled += instance.OnSkill4;
            }
        }
    }
    public HeroSkillsControlActions @HeroSkillsControl => new HeroSkillsControlActions(this);
    private int m_DefaultSchemeIndex = -1;
    public InputControlScheme DefaultScheme
    {
        get
        {
            if (m_DefaultSchemeIndex == -1) m_DefaultSchemeIndex = asset.FindControlSchemeIndex("Default");
            return asset.controlSchemes[m_DefaultSchemeIndex];
        }
    }
    public interface IHeroMoveControlActions
    {
        void OnMoveClick(InputAction.CallbackContext context);
        void OnMovePosition(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnAttackClick(InputAction.CallbackContext context);
    }
    public interface ICameraControlActions
    {
        void OnWASD(InputAction.CallbackContext context);
        void OnTrackPlayerHero(InputAction.CallbackContext context);
    }
    public interface IHeroSkillsControlActions
    {
        void OnSkill1(InputAction.CallbackContext context);
        void OnSkill2(InputAction.CallbackContext context);
        void OnSkill3(InputAction.CallbackContext context);
        void OnSkill4(InputAction.CallbackContext context);
    }
}
