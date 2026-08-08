using System;
using Cheat.core;
using SDG.Unturned;
using UnityEngine;

namespace Cheat.modules
{
	// Token: 0x02000014 RID: 20
	public class hkDamageTool
	{
		// Token: 0x06000072 RID: 114 RVA: 0x000068D8 File Offset: 0x00004AD8
		public static RaycastInfo Ov_raycast(Ray ray, float range, int mask, Player ignorePlayer)
		{
			Cheat.core.Main instance = Cheat.core.Main.Instance;
			Aimbot aimbot = ((instance == null) ? null : instance.aimbot);
			if (aimbot != null && aimbot.silentAimEnabled)
			{
				object silentTarget = aimbot.GetSilentTarget(ray);
				if (silentTarget != null)
				{
					Component component = silentTarget as Component;
					Transform transform = ((component == null) ? null : component.transform);
					if (transform != null)
					{
						ELimb elimb = (aimbot.silentAlwaysHead ? ELimb.LEFT_BACK : ELimb.LEFT_FOOT);
						Transform limb = Utils.GetLimb(transform, elimb);
						if (limb != null)
						{
							RaycastInfo raycastInfo = new RaycastInfo(limb);
							raycastInfo.point = limb.position + Vector3.up * 0.1f;
							raycastInfo.direction = ray.direction;
							raycastInfo.distance = Vector3.Distance(ray.origin, raycastInfo.point);
							raycastInfo.normal = -ray.direction;
							Player player = silentTarget as Player;
							if (player == null)
							{
								Zombie zombie = silentTarget as Zombie;
								if (zombie != null)
								{
									raycastInfo.zombie = zombie;
									raycastInfo.limb = (ELimb)elimb;
									raycastInfo.materialName = ((!zombie.isRadioactive) ? "Flesh_Dynamic" : "Alien_Dynamic");
									raycastInfo.material = ((!zombie.isRadioactive) ? (EPhysicsMaterial)7 : (EPhysicsMaterial)20);
								}
							}
							else
							{
								raycastInfo.player = player;
								raycastInfo.limb = (ELimb)elimb;
								raycastInfo.materialName = "Flesh_Dynamic";
								raycastInfo.material = (EPhysicsMaterial)7;
							}
							return raycastInfo;
						}
					}
				}
			}
			RaycastHit raycastHit;
			Physics.Raycast(ray, out raycastHit, range, mask, QueryTriggerInteraction.UseGlobal);
			RaycastInfo raycastInfo2 = new RaycastInfo(raycastHit);
			raycastInfo2.direction = ray.direction;
			if (raycastInfo2.transform != null)
			{
				raycastInfo2.player = DamageTool.getPlayer(raycastInfo2.transform);
				if (raycastInfo2.player == ignorePlayer)
				{
					raycastInfo2.player = null;
				}
				raycastInfo2.zombie = DamageTool.getZombie(raycastInfo2.transform);
				raycastInfo2.animal = DamageTool.getAnimal(raycastInfo2.transform);
				raycastInfo2.vehicle = DamageTool.getVehicle(raycastInfo2.transform);
				raycastInfo2.limb = DamageTool.getLimb(raycastInfo2.transform);
				if (raycastInfo2.zombie != null && raycastInfo2.zombie.isRadioactive)
				{
					raycastInfo2.materialName = "Alien_Dynamic";
					raycastInfo2.material = (EPhysicsMaterial)20;
				}
				else if (!(raycastInfo2.player != null) && !(raycastInfo2.zombie != null) && !(raycastInfo2.animal != null))
				{
					raycastInfo2.materialName = PhysicsTool.GetMaterialName(raycastHit.point, raycastInfo2.transform, raycastInfo2.collider);
				}
				else
				{
					raycastInfo2.materialName = "Flesh_Dynamic";
					raycastInfo2.material = (EPhysicsMaterial)7;
				}
			}
			return raycastInfo2;
		}
	}
}
