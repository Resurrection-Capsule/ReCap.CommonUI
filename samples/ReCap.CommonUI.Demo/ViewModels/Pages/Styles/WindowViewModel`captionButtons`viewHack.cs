#if DROP_OUTSIDE_HACK
using System;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using ReCap.CommonUI.Attached.WindowChrome;

namespace ReCap.CommonUI.Demo.ViewModels.Pages.Styles
{
    partial class WindowViewModel
    {
        partial class CaptionButtonsSide
        {
            WeakReference<ItemsControl> _itemsCtl = null;
            Point _prevPoint = new(-1d, -1d);
            public void Attach(ItemsControl itemsCtl)
            {
                if ((_itemsCtl != null) && _itemsCtl.TryGetTarget(out _))
                    return;
                if (itemsCtl == null)
                    return;

                itemsCtl.DetachedFromVisualTree += ItemsControl_DetachedFromVisualTree;
#if !NO
                itemsCtl.PointerPressed += ItemsControl_PointerPressed;
                itemsCtl.PointerReleased += ItemsControl_PointerReleased;
                //itemsCtl.PointerMoved += ItemsControl_PointerMoved;
                /*
                itemsCtl.AddHandler(InputElement.PointerPressedEvent,  ItemsControl_PointerPressed,  _ROUTING_STRATS, true);
                itemsCtl.AddHandler(InputElement.PointerReleasedEvent, ItemsControl_PointerReleased, _ROUTING_STRATS);
                //itemsCtl.AddHandler(InputElement.PointerMovedEvent,    ItemsControl_PointerMoved,    _ROUTING_STRATS);
                */
#else
                AddItemsControlHandler(ref itemsCtl, InputElement.PointerPressedEvent,  ItemsControl_PointerPressed);
                AddItemsControlHandler(ref itemsCtl, InputElement.PointerReleasedEvent, ItemsControl_PointerReleased);
                //AddItemsControlHandler(ref itemsCtl, InputElement.PointerMovedEvent,    ItemsControl_PointerMoved);
#endif
                _itemsCtl = new(itemsCtl);
            }

            public void Detach()
            {
                if (_itemsCtl == null)
                    return;
                if (!_itemsCtl.TryGetTarget(out ItemsControl itemsCtl))
                    return;

                itemsCtl.DetachedFromVisualTree -= ItemsControl_DetachedFromVisualTree;
#if !NO
                itemsCtl.PointerPressed -= ItemsControl_PointerPressed;
                itemsCtl.PointerReleased -= ItemsControl_PointerReleased;
                //itemsCtl.PointerMoved -= ItemsControl_PointerMoved;
#else
                RemoveItemsControlHandler(ref itemsCtl, InputElement.PointerPressedEvent,  ItemsControl_PointerPressed);
                RemoveItemsControlHandler(ref itemsCtl, InputElement.PointerReleasedEvent, ItemsControl_PointerReleased);
                //RemoveItemsControlHandler(ref itemsCtl, InputElement.PointerMovedEvent,    ItemsControl_PointerMoved);
#endif
                _itemsCtl = null;
            }


            const RoutingStrategies _ROUTING_STRATS = RoutingStrategies.Direct | RoutingStrategies.Tunnel | RoutingStrategies.Bubble;
            static void AddItemsControlHandler<TEventArgs>(ref ItemsControl itemsCtl, RoutedEvent<TEventArgs> routedEvent, EventHandler<TEventArgs> handler)
                where TEventArgs : RoutedEventArgs
                => itemsCtl.AddHandler(routedEvent, handler, _ROUTING_STRATS, true);

            static void RemoveItemsControlHandler<TEventArgs>(ref ItemsControl itemsCtl, RoutedEvent<TEventArgs> routedEvent, EventHandler<TEventArgs> handler)
                where TEventArgs : RoutedEventArgs
                => itemsCtl.RemoveHandler(routedEvent, handler);


            void ItemsControl_DetachedFromVisualTree(object sender, VisualTreeAttachmentEventArgs e)
                => Detach();


            bool _isLeftButtonPressed = false;
            bool _isRightButtonPressed = false;
            int _prevPointerOverIndex = -1;
            void ItemsControl_PointerPressed(object sender, PointerPressedEventArgs e)
            {
                if (e.Properties.IsLeftButtonPressed)
                    _isLeftButtonPressed = true;

                if (e.Properties.IsRightButtonPressed)
                    _isRightButtonPressed = true;

                _prevPointerOverIndex = TryGetItemContainingPoint(sender as ItemsControl, e, true, out int pointerOverIndex)
                    ? pointerOverIndex
                    : -1
                ;
            }
            void ItemsControl_PointerReleased(object sender, PointerReleasedEventArgs e)
            {
                if (sender is not ItemsControl itemsCtl)
                    return;

                bool isLeftButtonPressed = _isLeftButtonPressed;
                bool isRightButtonPressed = _isRightButtonPressed;
#if !NO

                if (!e.Properties.IsLeftButtonPressed)
                    _isLeftButtonPressed = false;

                if (!e.Properties.IsRightButtonPressed)
                    _isRightButtonPressed = false;

                Dispatcher.UIThread.Post(() => ItemsControl_PointerReleased_After(itemsCtl, e, isLeftButtonPressed, isRightButtonPressed));
            }

            void ItemsControl_PointerReleased_After(ItemsControl itemsCtl, PointerReleasedEventArgs e, bool isLeftButtonPressed, bool isRightButtonPressed)
            {
                //Debug.WriteLine($"{nameof(ItemsControl_PointerReleased_After)}({itemsCtl}, {e})");
#else
                //Debug.WriteLine($"{itemsCtl}, {e}");
#endif
                if (!_executeNext)
                {
                    _executeNext = true;
                    return;
                }

                if (_nextStAction != null)
                {
                    Debug.WriteLine($"{nameof(_nextStAction)} != null");
                    /*
                    if (IsInside(itemsCtl, e))
                    {
                    */
                        _nextStAction.StAction();
                    /*
                    }
                    else
                    {
                        int oldIdx = _nextStAction.OldIndex;
                        CaptionButtons.RemoveAt(oldIdx);
                    }
                    */
                    _nextStAction = null;
                    return;
                }


                /*
                Debug.WriteLine($"{nameof(_nextStAction)} == null");
                Debug.WriteLine($"{nameof(isRightButtonPressed)}: {isRightButtonPressed}");
                Debug.WriteLine($"{nameof(_isRightButtonPressed)}: {_isRightButtonPressed}");
                */
                /*
                */
                int removeAtIndex = -1;
                if (isRightButtonPressed)
                {
                    if (!TryGetItemContainingPoint(itemsCtl, e, true, out removeAtIndex))
                        return;
                    /*
                    {
                        if (_prevPointerOverIndex >= 0)
                            removeAtIndex = _prevPointerOverIndex;
                        else
                            return;
                    }
                    */
                }
                else
                {
                    if (TryGetItemContainingPoint(itemsCtl, e, false, out removeAtIndex))
                    {
                        if (removeAtIndex == _prevPointerOverIndex)
                            return;
                    }
                    else
                    {
                        double itemsCtlPointX = e.GetPosition(itemsCtl).X;
                        if (itemsCtlPointX < 0)
                            removeAtIndex = 0;
                        else if (itemsCtlPointX > itemsCtl.Bounds.Size.Width)
                            removeAtIndex = itemsCtl.ItemsPanelRoot.Children.Count - 1;
                        else
                            return;
                    }
                }
                


                if (removeAtIndex < 0)
                    return;

                bool remove = false;
                string removeStr = null;
                if (isRightButtonPressed)
                {
                    remove = true;
                    removeStr = "Right mouse-button pressed. ";
                }
                else if (!IsInside(itemsCtl, e, print: true))
                {
                    remove = true;
                    removeStr = $"Pointer outside {nameof(ItemsControl)}. ";
                }

                if (remove)
                {
                    CaptionButtons.RemoveAt(removeAtIndex);
                    Debug.WriteLine($"{removeStr}Removed [{removeAtIndex}].");
                }
            }

            static bool TryGetItemContainingPoint(ItemsControl itemsCtl, PointerEventArgs e, out int idx)
                => TryGetItemContainingPoint(itemsCtl, e, true, out idx);
            static bool TryGetItemContainingPoint(ItemsControl itemsCtl, PointerEventArgs e, bool y, out int idx)
            {
                var children = itemsCtl.ItemsPanelRoot.Children.ToArray();
                int childCount = children.Length;
                if (childCount <= 0)
                    goto fail;

                for (int i = 0; i < childCount; i++)
                {
                    var child = children[i];
                    if (IsInside(child, e, y: y))
                    {
                        idx = i;
                        return true;
                    }
                }

                fail:
                idx = -1;
                return false;
            }


            const bool _DEFAULT_IsInside_y = true;
            const bool _DEFAULT_IsInside_print = false;
            static bool IsInside(Visual visual, PointerEventArgs e, bool y = _DEFAULT_IsInside_y, bool print = _DEFAULT_IsInside_print)
                => IsInside(visual, e.GetPosition(visual), y, print);
            static bool IsInside(Visual visual, Point point, bool y = _DEFAULT_IsInside_y, bool print = _DEFAULT_IsInside_print)
            {
                var bounds = visual.Bounds;
                Size size = bounds.Size;
                double ptX = point.X;
                double ptY = point.Y;
                if (print)
                {
                    Debug.WriteLine($"{nameof(bounds)}: {bounds}");
                    Debug.WriteLine($"{nameof(size)}: {size}");
                    Debug.WriteLine($"{nameof(point)}: {point}");
                }


                if (ptX > size.Width)
                    return false;
                else if (ptX < 0)
                    return false;
                else if (!y)
                    return true;
                else if (ptY > size.Height)
                    return false;
                else if (ptY < 0)
                    return false;
                else
                    return true;
            }

            static bool TryGetAsInputElement(object source, out InputElement inputElement)
            {
                if (source == null)
                    goto fail;
                else if (source is InputElement inputEl)
                {
                    inputElement = inputEl;
                    return true;
                }
                else
                    goto fail;
                /*
                string propStr = $"{nameof(PointerReleasedEventArgs)}.{nameof(PointerReleasedEventArgs.Source)}";
                string sourceStr;
                if (source != null)
                {
                    if (source is InputElement inputEl)
                    {
                        sourceStr = nameof(InputElement);
                        string elName = inputEl.Name;
                        if (!string.IsNullOrWhiteSpace(elName))
                            sourceStr += $" '{elName}'";
                    }
                    else
                    {
                        sourceStr = source.GetType().FullName;
                    }
                }
                else
                {
                    sourceStr = "null";
                }
                Debug.WriteLine($"{propStr}: ({sourceStr})");
                */
                fail:
                inputElement = null;
                return false;
            }


            /*
            void ItemsControl_PointerMoved(object sender, PointerEventArgs e)
            {
                if (!_isLeftButtonPressed)
                    return;

                ItemsControl itemsCtl = (ItemsControl)sender;
                Point point = e.GetPosition(itemsCtl);
                if (point == _prevPoint)
                    return;

                Debug.WriteLine($"'{itemsCtl.Name}': '{point}'");

                _prevPoint = point;
            }
            */
        }
    }
}
#endif