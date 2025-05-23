# 카페인 없이는 못살아!


## 📖 개요
`3D_basicDungeon`은 Unity 엔진을 이용한 3D 던전 탐험 기초 프로젝트입니다.
기본적인 체력 부분은 카페인으로 표기하여, 커피를 마셔야 카페인 지수가 올라가 생존할 수 있습니다.

플레이어 이동, 점프, 동적 환경 조사, 점프대, 아이템 획득·인벤토리, 장착, 사용, 간단한 UI와 낮/밤 사이클을 구현했습니다.

---

## 🚀 주요 기능
### 1. 캐릭터 컨트롤
- **이동(WASD)**, **마우스 시점 조작**(마우스 이동)  
- **점프(스페이스)**: `Rigidbody` + `ForceMode.Impulse`  
- **낙하 속도 가속**: 바닥에 닿지 않을 때 중력 계수 적용
- **인벤토리(E)**: 인벤토리 창 열기
- **아이템 파밍(F)**: 소지 가능한 아이템을 인벤토리에 저장

### 2. 동적 환경 조사
- **Raycast**(카메라 중앙)로 바라보는 오브젝트 감지  
- **상호작용 프롬프트** UI(TextMeshPro)로 이름·설명 표시

### 3. 점프대
- **OnCollisionEnter**: 밟으면 순간 임펄스 힘 가하기  
- 높은 곳으로 튕기는 발판 구현
- 다른 방으로 날아서 이동하는 식의 연출

### 4. 아이템 & 인벤토리
- **ScriptableObject**로 아이템 데이터(이름·설명·아이콘·프리팹·소비 효과·스택 여부) 관리
- 맵에 배치된 **ItemObject**(IInteractable) 클릭(`F`) 시 획득  
- **Inventory UI**(`E`):  
  - 4×3 슬롯  
  - 아이템 정보(이름·설명·스탯) 표시  
  - 사용·장착·해제·버리기 버튼  
- **장착**: 장착용 카메라 상 위치 - 프리팹 인스턴스화 · 카메라 레이어 분리  
- **소비 효과**: 카페인·배고픔 회복, 파워 효과(시간제)

### 5. UI
- **체력바(카페인), 배고픔바, 파워바**(Image Fill 방식)  
- **인벤토리 창, 조사 프롬프트**(TextMeshPro)  

### 6. 환경
- **NavMeshSurface**: 땅·장애물에 대한 베이크  
- **Day & Night Cycle**: Skybox 머티리얼 블렌드로 낮/밤 전환  
- **CampFire**: 주기적 애니메이션 및 라이팅  

---

## 🧾 사용한 에셋 목록

### 🎭 캐릭터 및 애니메이션  
**제공처**: [Mixamo](https://www.mixamo.com)
- **캐릭터**: [Mixamo 캐릭터 라이브러리 – 페이지 2](https://www.mixamo.com/#/?page=2&type=Character)
- **애니메이션 목록**:
  - `@Standing React Large From Right`
  - `@Standing Melee Combo Attack Ver. 2`
  - `@Great Sword Attack`
  - `@Jumping`
  - `@Standing W_Briefcase Idle`
  - `@Walking` 등

### Font: 무료 폰트 사용 (NEXON Lv1 Gothic)

### 🌄 배경/환경 에셋 (Environment)  
**Prototype Map (3D Environments)**
- **Asset Store**: Prototype Map  
- **URL**: https://assetstore.unity.com/packages/3d/environments/prototype-map-315588
- **Usage**: 공간 구현 오브젝트로 사용



### 🌌 스카이박스 (Skybox)  
**HDRI Pack (2D Textures & Materials)**
- **Asset Store**: HDRI Pack – Sky  
- **URL**: https://assetstore.unity.com/packages/2d/textures-materials/sky/hdri-pack-72511  
- **Usage**: 시간대별 스카이박스 머티리얼로 사용

---

## 🔧 사용된 프로그램 및 버전
**Unity 2022.3. 17 LTS** 사용
