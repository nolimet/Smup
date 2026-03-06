# Enemy ECS Collision Integration Guide

## Overview
This guide explains how to integrate your existing MonoBehaviour enemies with the ECS collision system so they can be damaged by ECS bullets.

## Two Approaches

### Approach 1: Pure ECS Enemies (Recommended for New Enemies)
Use `EnemyAuthoring` component for enemies that are fully converted to ECS.

### Approach 2: Hybrid Enemies (For Existing MonoBehaviour Enemies)
Use `EnemyECSBridge` component to make existing `Enemy.cs` MonoBehaviours work with ECS bullets.

---

## Approach 1: Pure ECS Enemies

### Step 1: Add EnemyAuthoring Component
1. Select your enemy prefab in Unity
2. Add Component → `EnemyAuthoring`
3. Configure the fields:
   - **Health**: Enemy max health
   - **Scrap Value**: Reward when killed
   - **Scrap Cloud Size**: Visual size of scrap
   - **Contact Damage**: Damage to player on collision
   - **Collider Radius**: Size of sphere collider (default: 0.5)
   - **Mass**: Physics mass (default: 1.0)
   - **Is Kinematic**: Check if enemy doesn't move from physics forces

### Step 2: Baking Process
Unity will automatically bake the GameObject into an ECS entity with:
- ✅ `EnemyTag` - Identifies as enemy
- ✅ `EnemyData` - Contains health, damage, etc.
- ✅ `PhysicsCollider` - Sphere collider for collision
- ✅ `PhysicsMass` - Physics mass properties
- ✅ `PhysicsVelocity` - Velocity component
- ✅ `LocalTransform` - Position in world

### Step 3: Collision Configuration
The baker automatically sets up collision layers:
```csharp
BelongsTo = 1u << 1,      // Enemy layer (bit 1)
CollidesWith = 1u << 0,   // Bullet layer (bit 0)
```

Make sure your bullets use matching collision filters:
```csharp
BelongsTo = 1u << 0,      // Bullet layer (bit 0)
CollidesWith = 1u << 1,   // Enemy layer (bit 1)
```

### Step 4: Test
- Spawn enemy entity
- Fire bullets at it
- Enemy should take damage and be destroyed when health reaches 0

---

## Approach 2: Hybrid Enemies (MonoBehaviour + ECS)

### Step 1: Add EnemyECSBridge Component
1. Select your enemy prefab with `Enemy.cs` component
2. Add Component → `EnemyECSBridge`
3. Set **Collider Radius** to match your enemy's size (default: 0.5)

### Step 2: How It Works
The bridge component:
1. **OnEnable/Start**: Creates an ECS entity with physics components
2. **Update**: Syncs position and health from MonoBehaviour to ECS every frame
3. **OnDisable/OnDestroy**: Removes the ECS entity

### Step 3: Damage Flow
```
ECS Bullet → Collision System → Damages ECS Entity
                                       ↓
                              Health reduced in EnemyData
                                       ↓
                              Bridge syncs to MonoBehaviour
                                       ↓
                              Enemy.ReceiveDamage() still works
```

### Step 4: Integration with Existing Code
Your existing `Enemy.cs` code continues to work:
- ✅ `Enemy.ReceiveDamage()` still functions
- ✅ Movement patterns work normally
- ✅ Attack patterns work normally
- ✅ Object pooling works normally
- ✅ Kill rewards work normally

The bridge just adds an invisible ECS "shadow" entity for collision detection.

---

## Collision Layer Configuration

### Current Setup
```
Layer 0 (bit 0): Bullets
Layer 1 (bit 1): Enemies
```

### To Change Layers
Edit the collision filters in:

**For Enemies** (`EnemyAuthoring.cs` line ~35):
```csharp
new CollisionFilter
{
    BelongsTo = 1u << YOUR_ENEMY_LAYER,
    CollidesWith = 1u << YOUR_BULLET_LAYER,
    GroupIndex = 0
}
```

**For Bullets** (`BulletLibraryBootstrapSystem.cs` line ~88):
```csharp
new CollisionFilter
{
    BelongsTo = 1u << YOUR_BULLET_LAYER,
    CollidesWith = 1u << YOUR_ENEMY_LAYER,
    GroupIndex = 0
}
```

---

## Files Created

### Core Systems
1. **EnemyAuthoring.cs** (Updated)
   - Bakes enemies to ECS entities
   - Adds physics components
   - Configurable collider size and mass

2. **HybridEnemyBridgeSystem.cs** (New)
   - ECS system that manages hybrid enemies
   - Creates/updates/removes entities for MonoBehaviours
   - Provides API for manual entity management

3. **EnemyECSBridge.cs** (New)
   - MonoBehaviour component for hybrid approach
   - Automatically syncs with ECS every frame
   - Handles entity lifecycle

4. **EnemyHealthSystem.cs** (New)
   - Monitors enemy health in ECS
   - Can be extended for additional health logic

---

## Testing Checklist

### For Pure ECS Enemies
- [ ] Enemy prefab has `EnemyAuthoring` component
- [ ] Health value is set (> 0)
- [ ] Collider radius matches enemy visual size
- [ ] Spawn enemy in scene
- [ ] Fire bullets at enemy
- [ ] Enemy takes damage (health decreases)
- [ ] Enemy is destroyed when health reaches 0

### For Hybrid Enemies
- [ ] Enemy prefab has both `Enemy.cs` and `EnemyECSBridge`
- [ ] Collider radius is configured
- [ ] Enemy spawns normally with your existing system
- [ ] ECS entity is created automatically (check Entity Debugger)
- [ ] Bullets collide with enemy
- [ ] Enemy health decreases
- [ ] Enemy is destroyed normally via `Kill()` method

---

## Debugging

### Check if ECS Entity Exists
```
Window > Entities > Hierarchy
Look for entities with EnemyTag component
```

### Check Collision Filters
Add logging in `BulletEnemyCollisionSystem.cs`:
```csharp
Debug.Log($"Bullet collided with entity: {hitEntity}");
```

### Check Physics World
```
Window > Analysis > Profiler > Physics
Verify entities appear in physics world
```

### Common Issues

**Issue: No collisions detected**
- Check collision layer configuration matches
- Verify enemy has `PhysicsCollider` component
- Verify bullet has `PhysicsCollider` component
- Check collider sizes aren't too small

**Issue: Hybrid enemy not creating ECS entity**
- Check `World.DefaultGameObjectInjectionWorld` exists
- Verify `HybridEnemyBridgeSystem` is registered
- Check for errors in console

**Issue: Enemy health not syncing**
- Verify `EnemyECSBridge.Update()` is running
- Check `_bridgeSystem` is not null
- Ensure enemy is enabled

---

## Performance Notes

### Pure ECS Enemies
- ✅ Best performance (no MonoBehaviour overhead)
- ✅ Fully Burst-compiled collision detection
- ✅ Recommended for 100+ enemies

### Hybrid Enemies
- ⚠️ Some overhead from position/health syncing
- ⚠️ MonoBehaviour Update() called every frame
- ✅ Good for <50 enemies or mixed gameplay
- ✅ Easier migration path for existing code

---

## Migration Path: MonoBehaviour → Pure ECS

If you want to gradually convert enemies to pure ECS:

1. **Start**: Add `EnemyECSBridge` to existing enemies
2. **Test**: Verify collisions work with hybrid approach
3. **Migrate Logic**: Move movement/attack to ECS systems
4. **Convert**: Replace with `EnemyAuthoring` component
5. **Cleanup**: Remove old MonoBehaviour code

---

## Example: Manual Entity Creation

If you need manual control:

```csharp
// Get the bridge system
var world = World.DefaultGameObjectInjectionWorld;
var bridge = world.GetOrCreateSystemManaged<HybridEnemyBridgeSystem>();

// Create entity for enemy
Enemy myEnemy = GetComponent<Enemy>();
Entity entity = bridge.CreateEnemyEntity(myEnemy, colliderRadius: 0.5f);

// Update position
bridge.SyncEnemyPosition(entity, transform.position);

// Update health
bridge.SyncEnemyHealth(entity, myEnemy.Health);

// Remove entity
bridge.RemoveEnemyEntity(entity);
```

---

## Summary

✅ **Pure ECS**: Use `EnemyAuthoring` for new enemies
✅ **Hybrid**: Use `EnemyECSBridge` for existing `Enemy.cs` MonoBehaviours
✅ Both approaches work with the same collision system
✅ Performance optimized for 30k+ bullets

Choose the approach that best fits your workflow!

