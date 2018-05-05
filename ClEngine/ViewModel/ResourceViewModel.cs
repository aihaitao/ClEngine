using System.Collections.Generic;
using ClEngine.CoreLibrary.Asset;
using ClEngine.Model;
using GalaSoft.MvvmLight;

namespace ClEngine.ViewModel
{
	public class ResourceViewModel : ViewModelBase
	{
		public readonly List<ResourceModel> ResourceTypeList;
		private ResourceModel ImageResourceModel;
		private ResourceModel AnimationResouceModel;
		private ResourceModel SoundResourceModel;
		private ResourceModel ParticleResourceModel;
		private ResourceModel FontResourceModel;

		public ResourceViewModel()
		{
			ImageResourceModel = new ResourceModel {Name = "图片", Type = ResourceType.Image};
			AnimationResouceModel = new ResourceModel {Name = "动画", Type = ResourceType.Animation};
			SoundResourceModel = new ResourceModel {Name = "音效", Type = ResourceType.Sound};
			ParticleResourceModel = new ResourceModel {Name = "粒子", Type = ResourceType.Particle};
			FontResourceModel = new ResourceModel {Name = "文字", Type = ResourceType.Font};

			ResourceTypeList = new List<ResourceModel>
			{
				ImageResourceModel,
				AnimationResouceModel,
				SoundResourceModel,
				ParticleResourceModel,
				FontResourceModel,
			};
		}

		public void SetImageModel(List<ResourceInfo> resourceInfo)
		{
			ImageResourceModel.Resources = resourceInfo;
		}

		public void SetAnimationModel(List<ResourceInfo> resourceInfo)
		{
			AnimationResouceModel.Resources = resourceInfo;
		}
		public void SetSoundModel(List<ResourceInfo> resourceInfo)
		{
			SoundResourceModel.Resources = resourceInfo;
		}
		public void SetParticleModel(List<ResourceInfo> resourceInfo)
		{
			ParticleResourceModel.Resources = resourceInfo;
		}
		public void SetFontModel(List<ResourceInfo> resourceInfo)
		{
			FontResourceModel.Resources = resourceInfo;
		}
	}
}