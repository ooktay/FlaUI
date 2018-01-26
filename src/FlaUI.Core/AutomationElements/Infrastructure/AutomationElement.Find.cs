﻿using System;
using System.Collections.Generic;
using System.Linq;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;
using FlaUI.Core.Tools;

namespace FlaUI.Core.AutomationElements.Infrastructure
{
    public partial class AutomationElement
    {
        /// <summary>
        /// Finds all elements in the given treescope and with the given condition within the given timeout.
        /// </summary>
        public AutomationElement[] FindAll(TreeScope treeScope, ConditionBase condition, TimeSpan? timeout = null)
        {
            bool WhilePredicate(AutomationElement[] elements) => elements.Length == 0;
            AutomationElement[] RetryMethod() => FrameworkAutomationElement.FindAll(treeScope, condition);
            return Retry.While(RetryMethod, WhilePredicate, timeout);
        }

        /// <summary>
        /// Finds the first element which is in the given treescope with the given condition within the given timeout period.
        /// </summary>
        public AutomationElement FindFirst(TreeScope treeScope, ConditionBase condition, TimeSpan? timeout = null)
        {
            bool WhilePredicate(AutomationElement element) => element == null;
            AutomationElement RetryMethod() => FrameworkAutomationElement.FindFirst(treeScope, condition);
            return Retry.While(RetryMethod, WhilePredicate, timeout);
        }

        /// <summary>
        /// Find all matching elements in the specified order.
        /// </summary>
        public AutomationElement[] FindAllWithOptions(TreeScope treeScope, ConditionBase condition,
            TreeTraversalOptions traversalOptions, AutomationElement root)
        {
            return FrameworkAutomationElement.FindAllWithOptions(treeScope, condition, traversalOptions, root);
        }

        /// <summary>
        /// Finds the first matching element in the specified order.
        /// </summary>
        public AutomationElement FindFirstWithOptions(TreeScope treeScope, ConditionBase condition,
            TreeTraversalOptions traversalOptions, AutomationElement root)
        {
            return FrameworkAutomationElement.FindFirstWithOptions(treeScope, condition, traversalOptions, root);
        }

        /// <summary>
        /// Finds the first element by iterating thru all conditions.
        /// </summary>
        public AutomationElement FindFirstNested(params ConditionBase[] nestedConditions)
        {
            var currentElement = this;
            foreach (var condition in nestedConditions)
            {
                currentElement = currentElement.FindFirstChild(condition);
                if (currentElement == null)
                {
                    return null;
                }
            }
            return currentElement;
        }

        /// <summary>
        /// Finds all elements by iterating thru all conditions.
        /// </summary>
        public AutomationElement[] FindAllNested(params ConditionBase[] nestedConditions)
        {
            var currentElement = this;
            for (var i = 0; i < nestedConditions.Length - 1; i++)
            {
                var condition = nestedConditions[i];
                currentElement = currentElement.FindFirstChild(condition);
                if (currentElement == null)
                {
                    return null;
                }
            }
            return currentElement.FindAllChildren(nestedConditions.Last());
        }

        /// <summary>
        /// Finds for the first item which matches the given xpath.
        /// </summary>
        public AutomationElement FindFirstByXPath(string xPath)
        {
            var xPathNavigator = new AutomationElementXPathNavigator(this);
            var nodeItem = xPathNavigator.SelectSingleNode(xPath);
            return (AutomationElement)nodeItem?.UnderlyingObject;
        }

        /// <summary>
        /// Finds all items which match the given xpath.
        /// </summary>
        public AutomationElement[] FindAllByXPath(string xPath)
        {
            var xPathNavigator = new AutomationElementXPathNavigator(this);
            var itemNodeIterator = xPathNavigator.Select(xPath);
            var itemList = new List<AutomationElement>();
            while (itemNodeIterator.MoveNext())
            {
                var automationItem = (AutomationElement)itemNodeIterator.Current.UnderlyingObject;
                itemList.Add(automationItem);
            }
            return itemList.ToArray();
        }

        /// <summary>
        /// Finds the element which is in the given treescope with the given condition and the given index within the given timeout period.
        /// </summary>
        public AutomationElement FindAt(TreeScope treeScope, int index, ConditionBase condition, TimeSpan? timeout = null)
        {
            bool WhilePredicate(AutomationElement element) => element == null;
            AutomationElement RetryMethod() => FrameworkAutomationElement.FindIndexed(treeScope, index, condition);
            return Retry.While(RetryMethod, WhilePredicate, timeout);
        }

        /// <summary>
        /// Finds the first child.
        /// </summary>
        /// <returns>The found element or null if no element was found.</returns>
        public AutomationElement FindFirstChild()
        {
            return FindFirst(TreeScope.Children, TrueCondition.Default);
        }

        /// <summary>
        /// Finds the first child with the given automation id.
        /// </summary>
        /// <param name="automationId">The automation id.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public AutomationElement FindFirstChild(string automationId)
        {
            return FindFirst(TreeScope.Children, ConditionFactory.ByAutomationId(automationId));
        }

        /// <summary>
        /// Finds the first child with the condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public AutomationElement FindFirstChild(ConditionBase condition)
        {
            return FindFirst(TreeScope.Children, condition);
        }

        /// <summary>
        /// Finds the first child with the condition.
        /// </summary>
        /// <param name="conditionFunc">The condition method.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public AutomationElement FindFirstChild(Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            var condition = conditionFunc(ConditionFactory);
            return FindFirstChild(condition);
        }

        /// <summary>
        /// Finds all children.
        /// </summary>
        /// <returns>The found elements or an empty list if no elements were found.</returns>
        public AutomationElement[] FindAllChildren()
        {
            return FindAll(TreeScope.Children, TrueCondition.Default);
        }

        /// <summary>
        /// Finds all children with the condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>The found elements or an empty list if no elements were found.</returns>
        public AutomationElement[] FindAllChildren(ConditionBase condition)
        {
            return FindAll(TreeScope.Children, condition);
        }

        /// <summary>
        /// Finds all children with the condition.
        /// </summary>
        /// <param name="conditionFunc">The condition mehtod.</param>
        /// <returns>The found elements or an empty list if no elements were found.</returns>
        public AutomationElement[] FindAllChildren(Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            var condition = conditionFunc(ConditionFactory);
            return FindAllChildren(condition);
        }

        /// <summary>
        /// Finds the first descendant.
        /// </summary>
        /// <returns>The found element or null if no element was found.</returns>
        public AutomationElement FindFirstDescendant()
        {
            return FindFirst(TreeScope.Descendants, TrueCondition.Default);
        }

        /// <summary>
        /// Finds the first descendant with the given automation id.
        /// </summary>
        /// <param name="automationId">The automation id.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public AutomationElement FindFirstDescendant(string automationId)
        {
            return FindFirst(TreeScope.Descendants, ConditionFactory.ByAutomationId(automationId));
        }

        /// <summary>
        /// Finds the first descendant with the condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public AutomationElement FindFirstDescendant(ConditionBase condition)
        {
            return FindFirst(TreeScope.Descendants, condition);
        }

        /// <summary>
        /// Finds the first descendant with the condition.
        /// </summary>
        /// <param name="conditionFunc">The condition method.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public AutomationElement FindFirstDescendant(Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            var condition = conditionFunc(ConditionFactory);
            return FindFirstDescendant(condition);
        }

        /// <summary>
        /// Finds all descendants.
        /// </summary>
        /// <returns>The found elements or an empty list if no elements were found.</returns>
        public AutomationElement[] FindAllDescendants()
        {
            return FindAll(TreeScope.Descendants, TrueCondition.Default);
        }

        /// <summary>
        /// Finds all descendants with the condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>The found elements or an empty list if no elements were found.</returns>
        public AutomationElement[] FindAllDescendants(ConditionBase condition)
        {
            return FindAll(TreeScope.Descendants, condition);
        }

        /// <summary>
        /// Finds all descendants with the condition.
        /// </summary>
        /// <param name="conditionFunc">The condition mehtod.</param>
        /// <returns>The found elements or an empty list if no elements were found.</returns>
        public AutomationElement[] FindAllDescendants(Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            var condition = conditionFunc(ConditionFactory);
            return FindAllDescendants(condition);
        }

        /// <summary>
        /// Finds the first element by iterating thru all conditions.
        /// </summary>
        /// <param name="conditionFunc">The condition method.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public AutomationElement FindFirstNested(Func<ConditionFactory, IList<ConditionBase>> conditionFunc)
        {
            var conditions = conditionFunc(ConditionFactory);
            return FindFirstNested(conditions.ToArray());
        }

        /// <summary>
        /// Finds all elements by iterating thru all conditions.
        /// </summary>
        /// <param name="conditionFunc">The condition method.</param>
        /// <returns>The found elements or an empty list if no elements were found.</returns>
        public AutomationElement[] FindAllNested(Func<ConditionFactory, IList<ConditionBase>> conditionFunc)
        {
            var conditions = conditionFunc(ConditionFactory);
            return FindAllNested(conditions.ToArray());
        }

        /// <summary>
        /// Finds the child at the given position with the condition.
        /// </summary>
        /// <param name="index">The index of the child to find.</param>
        /// <param name="condition">The condition.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public AutomationElement FindChildAt(int index, ConditionBase condition = null)
        {
            return FindAt(TreeScope.Children, index, condition ?? TrueCondition.Default);
        }

        /// <summary>
        /// Finds the child at the given position with the condition.
        /// </summary>
        /// <param name="index">The index of the child to find.</param>
        /// <param name="conditionFunc">The condition mehtod.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public AutomationElement FindChildAt(int index, Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            var condition = conditionFunc(ConditionFactory);
            return FindChildAt(index, condition);
        }
    }
}
