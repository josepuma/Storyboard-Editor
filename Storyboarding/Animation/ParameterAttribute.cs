using System;
namespace Storyboarding.Animation
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ParameterAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
