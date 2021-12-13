using Infrastructure.Core.DataAccess;
using Infrastructure.Core.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core.Providers
{
    public class LanguageProvider
    {
        static ConcurrentDictionary<string, Label> _labels = new ConcurrentDictionary<string,Label>();
        static UserRepository _userRepository = new UserRepository();

        public static string GetLabelCaption(string language, string labelName,string pageName)
        {
            Label labelValue;

            if (_labels.TryGetValue(language + labelName + pageName, out labelValue))
            {
                return labelValue.LabelText;
            }
            else
            {
                List<Label> labels = _userRepository.GetLabelForPage(labelName, pageName);

                if (labels != null && labels.Exists( l => l.LabelName == labelName && l.PageName == pageName))
                {

                    foreach (var item in labels)
                    {
                        _labels.TryAdd(item.Language + item.LabelName + item.PageName, item);
                    }

                    return GetLabelCaption(language, labelName, pageName);
                }
                else
                {

                    return language + labelName ;
                }

                
            }

        }

        public void ClearLanguageCache()
        {
            _labels.Clear();
        }

    }
}
